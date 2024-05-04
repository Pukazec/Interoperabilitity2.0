using BePart.Data;
using Dtos;
using System.Text.Json;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

namespace BePart.Data_Service
{
    public interface IZeebeService
    {
        public Task<ITopology> Status();

        public Task<IDeployResourceResponse> Deploy(string modelFilename);

        public Task<string> StartWorkflowInstance(string bpmProcessId);

        public void StartWorkers();
    }

    public class ZeebeeService : IZeebeService
    {
        private readonly IZeebeClient _client;
        private readonly IServiceProvider _serviceProvider;

        public ZeebeeService(IServiceProvider serviceProvider)
        {
            var authServer = "https://login.cloud.camunda.io/oauth/token";
            var clientId = "dytHHc0RGG-k.WQiyQwzUhGB_991MP2T";
            var clientSecret = "o-dY5o0jOBO~ui95XLqzGQ5c2-UWtE4vSy03_mOoZ_HRayRdTWLEqyLrQyT5pLIv";
            var zeebeUrl = "52ebf02c-001e-4c51-ba95-3836bf091c3b.bru-2.zeebe.camunda.io:443";
            char[] port =
            [
                '4', '3', ':'
            ];
            var audience = zeebeUrl.TrimEnd(port);

            _client = ZeebeClient
                .Builder()
                .UseGatewayAddress(zeebeUrl)
                .UseTransportEncryption()
                .UseAccessTokenSupplier(
                    CamundaCloudTokenProvider
                        .Builder()
                        .UseAuthServer(authServer)
                        .UseClientId(clientId)
                        .UseClientSecret(clientSecret)
                        .UseAudience(audience)
                        .Build())
                .Build();
            _serviceProvider = serviceProvider;
        }

        public void StartWorkers()
        {
            CreateGetCatsWorker();
        }

        public void CreateGetCatsWorker()
        {
            CreateWorker("get-cats", async (client, job) =>
            {
                await Console.Out.WriteLineAsync($"Recieved job: {job}");
                var noOfCats = job.Variables;
                using var scope = _serviceProvider.CreateScope();

                var filter = JsonSerializer.Deserialize<CatsAge>(noOfCats);

                var catService = scope.ServiceProvider.GetService<ICatService>();
                var catsCount = catService?.OlderThan(filter.catAge).Count;

                var variables = "{\"cats\":" + catsCount + "}";
                await Console.Out.WriteLineAsync($"Response: {variables}");

                await client
                    .NewCompleteJobCommand(job.Key)
                    .Variables(variables)
                    .Send();
            });
        }

        public void CreateMakeCatWorker()
        {
            CreateWorker(
                "make-cat", async (client, job) =>
            {
                await Console.Out.WriteLineAsync($"Make cat Recieved job: {job}");

                var cat = JsonSerializer.Deserialize<Cat>(job.Variables);

                using var scope = _serviceProvider.CreateScope();
                var catService = scope.ServiceProvider.GetService<ICatService>();
                if (cat != null)
                {
                    catService?.AddCat(cat);
                }

                await client
                    .NewCompleteJobCommand(job.Key)
                    .Send();
            });
        }

        public async Task<IDeployResourceResponse> Deploy(string modelFilename)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Resources", modelFilename);
            var deployment = await _client.NewDeployCommand().AddResourceFile(filename).Send();
            var result = deployment.Processes[0];
            await Console.Out.WriteLineAsync($"Deployed BPMN Model: {result?.BpmnProcessId} v. {result?.Version}");

            return deployment;
        }

        public async Task<string> StartWorkflowInstance(string bpmProcessId)
        {
            var instance = await _client.NewCreateProcessInstanceCommand()
                .BpmnProcessId(bpmProcessId)
                .LatestVersion()
                .WithResult()
                .Send();

            return JsonSerializer.Serialize(instance);
        }

        public Task<ITopology> Status()
        {
            return _client.TopologyRequest().Send();
        }

        private void CreateWorker(string jobType, JobHandler handleJob)
        {
            _client.NewWorker()
                .JobType(jobType)
                .Handler(handleJob)
                .MaxJobsActive(5)
                .Name(jobType)
                .PollInterval(TimeSpan.FromSeconds(50))
                .PollingTimeout(TimeSpan.FromSeconds(50))
                .Timeout(TimeSpan.FromHours(2))
                .Open();
        }
    }

    public class CatsAge
    {
        public int catAge { get; set; }
    }
}
