using SharedData;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace FePart
{
    public partial class AddHardwareWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly Hardware? _selectedHardware;

        public AddHardwareWindow(Hardware? selectedHardware, int? getById)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (selectedHardware != null)
            {
                _selectedHardware = selectedHardware;
                FillHardwareInformation();
            }
            else if (getById != null)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"Hardware/{getById}").Result;
                if (response.IsSuccessStatusCode)
                {
                    _selectedHardware = response.Content.ReadFromJsonAsync<Hardware>().Result;
                    FillHardwareInformation();
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            else
            {
                _selectedHardware = null;
            }
        }

        private void FillHardwareInformation()
        {
            if (_selectedHardware == null) return;

            name.Text = _selectedHardware.Name;
            type.Text = _selectedHardware.Type.ToString();
            code.Text = _selectedHardware.Code;
            stock.Text = _selectedHardware.Stock.ToString();
            price.Text = _selectedHardware.Price.ToString();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var dto = new Hardware()
            {
                Name = name.Text,
                Type = Enum.Parse<HardwareType>(type.Text),
                Code = code.Text,
                Stock = Convert.ToInt32(stock.Text),
                Price = Convert.ToDouble(price.Text),
            };

            HttpResponseMessage response = null;

            if (_selectedHardware != null)
            {
                dto.Id = _selectedHardware.Id;
                response = await _httpClient.PutAsync($"Hardware/{dto.Id}", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            }
            else
            {
                response = await _httpClient.PostAsync("Hardware", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            }

            if (response.IsSuccessStatusCode)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }
    }
}
