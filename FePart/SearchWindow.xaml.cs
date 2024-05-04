using Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace FePart
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private const string UriString = "https://localhost:7082/";
        private readonly HttpClient _httpClient;
        private Cat? _selectedCat;

        public SearchWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(UriString)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameSearch.Text.Length > 3)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"Cat/{txtNameSearch.Text}/injection").Result;
                try
                {
                    _selectedCat = response.Content.ReadFromJsonAsync<Cat>().Result;
                    FillCatInformation();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
        }

        private void FillCatInformation()
        {
            if (_selectedCat == null) return;

            catName.Text = _selectedCat.CatName;
            catAge.Text = _selectedCat.Age.ToString();
            catColor.Text = _selectedCat.Color;
            catSummary.Text = _selectedCat.Summary ?? "";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
