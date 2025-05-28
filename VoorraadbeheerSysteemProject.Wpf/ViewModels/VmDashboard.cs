using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Services.Sales;
using System.Diagnostics;
using VoorraadbeheerSysteemProject.Wpf.Services.Purchases;
using VoorraadbeheerSysteemProject.Wpf.Services;
using System.Globalization;
using VoorraadbeheerSysteemProject.Wpf.Models;
using System.Windows.Threading;
using VoorraadbeheerSysteemProject.Wpf.Services.Customers;
using VoorraadbeheerSysteemProject.Wpf.Services.Suppliers;
using System.Windows.Media;
using System.Threading;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmDashboard : VmBase
    {
        //Navigation Property
        private readonly NavigationStore _navigationStore;
        private readonly SalesRequests _salesRequests;
        private readonly PurchasesRequests _purchasesRequests;
        private readonly CustomersRequests _customerRequests;
        private readonly ApiService _productsRequests;



        private decimal _totalSales;
        private decimal _totalPurchases;
        private decimal _totalProducts;
        private decimal _totalCustomers;

        private string _email;
        public string Email
        {
            get => _email ?? (UserSession.Email ?? "Unknown");
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string TotalSales => _totalSales.ToString("C");
        public string TotalPurchases => _totalPurchases.ToString("C");
        public string TotalProducts => _totalProducts.ToString("N0");
        public string TotalCustomers => _totalCustomers.ToString("N0");


        //Constructor
        public VmDashboard(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _salesRequests = new SalesRequests(AppConfig.ApiUrl);
            _purchasesRequests = new PurchasesRequests(AppConfig.ApiUrl);
            _productsRequests = new ApiService(AppConfig.ApiUrl);
            _customerRequests = new CustomersRequests(AppConfig.ApiUrl);

            LogoutCommand = new ButtonCommand(Logout);

            // Initialize Labels with default values
            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };

            // Initialize BarSeries with default values
            barSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Sales",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.Blue
                },
                new ColumnSeries
                {
                    Title = "Purchases",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.DarkGreen
                }
            };

            // Initialize LineSeries with default values
            lineSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sales Count",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.Blue

                },
                new LineSeries
                {
                    Title = "Purchases Count",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.DarkGreen
                }
            };

            cercleSeries = new SeriesCollection
            {
               new PieSeries
                {
                    Title = "Sales",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.Blue,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2,
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Purchases",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.Green,
                    Stroke = Brushes.DarkGreen,
                    StrokeThickness = 2,
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<double> { 0 },
                    Fill = Brushes.Orange,
                    Stroke = Brushes.DarkOrange,
                    StrokeThickness = 2,
                    DataLabels = true
                }
            };

            // Set Y-axis formatter
            YFormatter = value => value.ToString("N");

            // Load dashboard data asynchronously
            //This avoids unnecessary marshaling back to the UI thread and improves responsiveness in background threads.
            Task.Run(async () => await LoadDashboardDataAsync().ConfigureAwait(false));
        }

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                // Get data for the current month for totals
                var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

                var dateDay = DateTime.Now;
                // Get monthly data for the last 6 months for charts
                var endDate = new DateTime(dateDay.AddMonths(3).Year, dateDay.AddMonths(3).Month, 1);
                var startDate = new DateTime(dateDay.AddMonths(-5).Year, dateDay.AddMonths(-5).Month, 1);

                // Get monthly summaries
                var monthlySummaries = await _salesRequests.GetMonthlySummaryAsync(startDate, endDate);

                // Update current month totals
                var currentMonth = monthlySummaries.FirstOrDefault(m =>
                    m.Year == DateTime.Now.Year && m.Month == DateTime.Now.ToString("MMM"));

                if (currentMonth != null)
                {
                    _totalSales = currentMonth.SalesAmount;
                    _totalPurchases = currentMonth.PurchasesAmount;
                }
                else
                {
                    // Fallback to direct API calls if monthly summary doesn't include current month
                    _totalSales = await _salesRequests.GetSumByPeriodAsync(currentMonthStart, currentMonthEnd);
                    _totalPurchases = await _purchasesRequests.GetSumPurchaseByPeriodAsync(currentMonthStart, currentMonthEnd);
                }

                // Get total products count
                _totalProducts = await _productsRequests.GetProductCountAsync();
                _totalCustomers = await _customerRequests.GetCustomersCountAsync();

                // Update UI properties
                OnPropertyChanged(nameof(TotalSales));
                OnPropertyChanged(nameof(TotalPurchases));
                OnPropertyChanged(nameof(TotalProducts));
                OnPropertyChanged(nameof(TotalCustomers));

                // Update chart data
                UpdateChartsWithMonthlySummaries(monthlySummaries);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
            }
        }

        private void UpdateChartsWithMonthlySummaries(IEnumerable<MonthlySummaryDTO> monthlySummaries)
        {
            try
            {
                if (monthlySummaries == null || !monthlySummaries.Any())
                {
                    Debug.WriteLine("No monthly summary data available for charts");
                    return;
                }
                var orderedSummaries = monthlySummaries
                .OrderBy(m => m.Year)
                //.ThenBy(m => DateTime.ParseExact(m.Month, "MMM", CultureInfo.CurrentCulture).Month)
                .ThenBy(m => DateTime.ParseExact(m.Month, "MMM", new CultureInfo("fr-FR")).Month)

                .ToList();

                // Prepare chart data
                var labels = orderedSummaries.Select(m => m.Month).ToArray();
                var salesValues = new ChartValues<double>(orderedSummaries.Select(m => (double)m.SalesAmount));
                var purchasesValues = new ChartValues<double>(orderedSummaries.Select(m => (double)m.PurchasesAmount));

                // Convert int values to ChartValues<double> for consistency
                var salesCountValues = new ChartValues<double>(orderedSummaries.Select(m => (double)m.SalesCount));
                var purchasesCountValues = new ChartValues<double>(orderedSummaries.Select(m => (double)m.PurchasesCount));

                var margeBenificiaire = new ChartValues<double>(orderedSummaries.Select(s => (double)s.SalesAmount - (double)s.PurchasesAmount));
                // Update UI collections
                Labels = labels;
                if (barSeries != null && barSeries.Count >= 2)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        barSeries[0].Values = salesCountValues;
                        barSeries[1].Values = purchasesCountValues;

                        lineSeries[0].Values = salesValues;
                        lineSeries[1].Values = purchasesValues;

                        cercleSeries[0].Values = salesValues;
                        cercleSeries[1].Values = purchasesValues;
                        cercleSeries[2].Values = margeBenificiaire;
                    });
                }

                //if (LineSeries != null && LineSeries.Count >= 2)
                //{
                //    App.Current.Dispatcher.Invoke(() =>
                //    {



                //    });
                //}

                //if (CercleSeries != null && CercleSeries.Count >= 2)
                //{
                //    App.Current.Dispatcher.Invoke(() =>
                //    {


                //    });
                //}

                // Notify property changed
                OnPropertyChanged(nameof(Labels));
                OnPropertyChanged(nameof(barSeries));
                OnPropertyChanged(nameof(lineSeries));
                OnPropertyChanged(nameof(cercleSeries));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating charts: {ex.Message}");
            }
        }

        //Using BarSeries for DashBoard
        public SeriesCollection barSeries { get; set; }
        //Using PieSeries for Dashboard
        public SeriesCollection cercleSeries { get; set; }
        //Using LinesSeries For DashBoard
        public SeriesCollection lineSeries { get; set; }
        //Arrays of Months
        public string[] Labels { get; set; }

        //Show Generic String Data
        public Func<double, string> YFormatter { get; set; }

        public ICommand LogoutCommand { get; }
        private void Logout(object obj)
        {
            UserSession.Clear();
            _navigationStore.CurrentViewModel = new VmUserLogin(_navigationStore);
        }
    }
}