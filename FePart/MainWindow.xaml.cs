using Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace FePart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private Cat? SelectedCat { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void GetData_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = _httpClient.GetAsync("Cat").Result;
            if (response.IsSuccessStatusCode)
            {
                var cats = response.Content.ReadFromJsonAsync<List<Cat>>().Result;
                catGrid.ItemsSource = cats;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void AddCat_Click(object sender, RoutedEventArgs e)
        {
            new AddCatWindow(null, null).Show();
        }

        private void EditCat_Click(object sender, RoutedEventArgs e)
        {
            new AddCatWindow(SelectedCat, null).Show();
        }

        private void ViewCat_Click(object sender, RoutedEventArgs e)
        {
            new AddCatWindow(null, SelectedCat?.Id).Show();
        }

        private void DeleteCat_Click(object sender, RoutedEventArgs e)
        {
            _httpClient.DeleteAsync($"Cat/{SelectedCat?.Id}");
        }

        private void RowSelected(object sender, RoutedEventArgs e)
        {
            if (catGrid.SelectedItem != null)
            {
                SelectedCat = (Cat)catGrid.SelectedItem;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnView.IsEnabled = true;
            }
            else
            {
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnView.IsEnabled = false;
            }
        }
    }
}