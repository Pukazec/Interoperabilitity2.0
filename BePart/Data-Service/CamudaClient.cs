using BePart.Data;
using DotEnv.Core;
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

        public ZeebeeService(IEnvReader envReader, IServiceProvider serviceProvider)
        {
            var authServer = "https://login.cloud.camunda.io/oauth/token"; //envReader.GetStringValue("ZEEBE_AUTHORIZATION_SERVER_URL");
            var clientId = "dytHHc0RGG-k.WQiyQwzUhGB_991MP2T"; //envReader.GetStringValue("ZEEBE_CLIENT_ID");
            var clientSecret = "o-dY5o0jOBO~ui95XLqzGQ5c2-UWtE4vSy03_mOoZ_HRayRdTWLEqyLrQyT5pLIv"; //envReader.GetStringValue("ZEEBE_CLIENT_SECRET");
            var zeebeUrl = "52ebf02c-001e-4c51-ba95-3836bf091c3b.bru-2.zeebe.camunda.io:443"; //envReader.GetStringValue("ZEEBE_ADDRESS");
            char[] port =
            {
                '4', '3', ':'
            };
            var audience = zeebeUrl?.TrimEnd(port);

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
            //CreateMakeCatWorker();
        }

        public void CreateGetCatsWorker()
        {
            _createWorker("get-cats", async (client, job) =>
            {
                await Console.Out.WriteLineAsync($"Recieved job: {job}");

                using var scope = _serviceProvider.CreateScope();
                var catService = scope.ServiceProvider.GetService<ICatService>();
                var catsCount = catService.GetAllCats().Count;
                await client
                    .NewCompleteJobCommand(job.Key)
                    .Variables("{\"cats\":" + catsCount + "}")
                    .Send();
            });
        }

        public void CreateMakeCatWorker()
        {
            _createWorker(
                "make-cat", async (client, job) =>
            {
                await Console.Out.WriteLineAsync($"Make cat Recieved job: {job}");

                var cat = JsonSerializer.Deserialize<Cat>(job.Variables);

                using var scope = _serviceProvider.CreateScope();
                var catService = scope.ServiceProvider.GetService<ICatService>();
                catService.AddCat(cat);

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

        private void _createWorker(string jobType, JobHandler handleJob)
        {
            _client.NewWorker()
                .JobType(jobType)
                .Handler(handleJob)
                .MaxJobsActive(5)
                .Name(jobType)
                .PollInterval(TimeSpan.FromSeconds(50))
                .PollingTimeout(TimeSpan.FromSeconds(50))
                .Timeout(TimeSpan.FromSeconds(10))
                .Open();
        }
    }
}
