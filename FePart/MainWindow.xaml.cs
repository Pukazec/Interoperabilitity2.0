using SharedData;
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
        private Hardware? SelectedHardware { get; set; }
        private Review? SelectedReview { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7082/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void GetData_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = _httpClient.GetAsync("Hardware").Result;
            if (response.IsSuccessStatusCode)
            {
                var hardwares = response.Content.ReadFromJsonAsync<List<Hardware>>().Result;
                dataGrid.ItemsSource = hardwares;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void AddHardware_Click(object sender, RoutedEventArgs e)
        {
            new AddHardwareWindow(null, null, null).Show();
        }

        private void EditHardware_Click(object sender, RoutedEventArgs e)
        {
            new AddHardwareWindow(SelectedHardware, null, null).Show();
        }

        private void ViewHardware_Click(object sender, RoutedEventArgs e)
        {
            new AddHardwareWindow(null, null, SelectedHardware.Id).Show();
        }

        private void DeleteHardware_Click(object sender, RoutedEventArgs e)
        {
            _httpClient.DeleteAsync($"Hardware/{SelectedHardware.Id}");
        }

        private void RowSelected(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                SelectedHardware = (Hardware)dataGrid.SelectedItem;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnView.IsEnabled = true;
                btnNewReview.IsEnabled = true;

                HttpResponseMessage response = _httpClient.GetAsync($"Review").Result;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var reviews = response.Content.ReadFromJsonAsync<List<Review>>().Result;
                        reviewsDataGrid.ItemsSource = reviews;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            else
            {
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnView.IsEnabled = false;
            }
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            new AddHardwareWindow(SelectedHardware, null, null).Show();
        }

        private void EditReview_Click(object sender, RoutedEventArgs e)
        {
            new AddHardwareWindow(null, SelectedReview, null).Show();
        }

        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            _httpClient.DeleteAsync($"Review/{SelectedReview.Id}");
        }

        private void ViewReview_Click(object sender, RoutedEventArgs e)
        {
            new AddHardwareWindow(null, SelectedReview, null).Show();
        }

        private void ReveiwSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                SelectedReview = (Review)reviewsDataGrid.SelectedItem;
                btnEditReview.IsEnabled = true;
                btnDeleteReview.IsEnabled = true;
                btnViewReview.IsEnabled = true;
            }
            else
            {
                btnEditReview.IsEnabled = false;
                btnDeleteReview.IsEnabled = false;
                btnViewReview.IsEnabled = false;
            }
        }
    }
}