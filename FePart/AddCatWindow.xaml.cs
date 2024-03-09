using Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace FePart
{
    /// <summary>
    /// Interaction logic for AddCatWindow.xaml
    /// </summary>
    public partial class AddCatWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly Cat? _selectedCat;

        public AddCatWindow(Cat? selectedCat)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (selectedCat != null)
            {
                _selectedCat = selectedCat;
                FillCatInformation();
            }
            else
            {
                _selectedCat = null;
            }
        }

        private void FillCatInformation()
        {
            if (_selectedCat == null) return;

            catName.Text = _selectedCat.Name;
            catAge.Text = _selectedCat.Age.ToString();
            catColor.Text = _selectedCat.Color;
            catSummary.Text = _selectedCat.Summary ?? "";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var dto = new Cat()
            {
                Age = Convert.ToDouble(catAge.Text),
                Color = catColor.Text,
                Name = catName.Text,
                Summary = catSummary.Text
            };

            HttpResponseMessage response = null;

            if (_selectedCat != null)
            {
                dto.Id = _selectedCat.Id;
                response = await _httpClient.PutAsync($"Cat/{dto.Id}", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            }
            else
            {
                response = await _httpClient.PostAsync("Cat", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
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
