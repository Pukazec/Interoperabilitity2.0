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
        private readonly Review? _selectedReview;

        public AddHardwareWindow(Hardware? selectedHardware, Review? selectedReview, int? getById)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (selectedHardware == null && selectedReview == null)
            {
                btnReview.IsEnabled = false;
            }

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
            if (selectedReview != null)
            {
                _selectedReview = selectedReview;
                FillReviewInformation();
            }
        }

        private void FillReviewInformation()
        {
            if (_selectedReview == null) return;

            title.Text = _selectedReview.Title;
            text.Text = _selectedReview.Text.ToString();
            rating.Text = _selectedReview.Rating.ToString();
            hardware.Text = _selectedReview.Hardware?.Name.ToString();
            hardwareId.Text = _selectedReview.HardwareId.ToString();
        }

        private void FillHardwareInformation()
        {
            if (_selectedHardware == null) return;

            name.Text = _selectedHardware.Name;
            type.Text = _selectedHardware.Type.ToString();
            code.Text = _selectedHardware.Code;
            stock.Text = _selectedHardware.Stock.ToString();
            price.Text = _selectedHardware.Price.ToString();
            hardwareId.Text = _selectedHardware.Id.ToString();
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

        private async void Add2_Click(object sender, RoutedEventArgs e)
        {
            var dto = new Review()
            {
                Title = title.Text,
                Text = text.Text,
                Rating = Convert.ToInt32(rating.Text),
                HardwareId = Convert.ToInt32(_selectedHardware.Id)
            };

            HttpResponseMessage response = null;

            if (_selectedReview != null)
            {
                dto.Id = _selectedReview.Id;
                response = await _httpClient.PutAsync($"Review/{dto.Id}", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            }
            else
            {
                response = await _httpClient.PostAsync("Review", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
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
