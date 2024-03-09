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

        public AddCatWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public AddCatWindow(Cat? selectedCat)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (selectedCat != null)
            {
                // Set the edit flag and selectedCat
                _selectedCat = selectedCat;
                FillCatInformation(); // Call method to fill in the cat information
            }
        }

        private void FillCatInformation()
        {
            // Ensure _selectedCat is not null
            if (_selectedCat == null) return;

            // Fill in the TextBoxes with the cat's information
            catName.Text = _selectedCat.Name;
            catAge.Text = _selectedCat.Age.ToString();
            catColor.Text = _selectedCat.Color;
            catSummary.Text = _selectedCat.Summary ?? ""; // Use empty string if Summary is null
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

            var response = await _httpClient.PostAsync("Cat", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
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
