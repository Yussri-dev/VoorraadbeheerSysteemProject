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

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmDashboard : VmBase
    {
        //Navigation Property
        private readonly NavigationStore _navigationStore;
        private readonly SalesRequests _salesRequests;
        private readonly PurchasesRequests _purchasesRequests;
        private readonly CustomersRequests _customerRequests;
        private readonly SupplierRequests _supplierRequests;
        private readonly ApiService _productsRequests;
        public string Email => UserSession.Email ?? "Unknown";

        private decimal _totalSales;
        private decimal _totalPurchases;
        private decimal _totalProducts;
        private decimal _totalCustomers;

        public string TotalSales => _totalSales.ToString("C");
        public string TotalPurchases => _totalPurchases.ToString("C");
        public string TotalProducts => _totalProducts.ToString("N0");
        public string TotalCustomers => _totalProducts.ToString("N0");

        //Constructor
        public VmDashboard(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _salesRequests = new SalesRequests(AppConfig.ApiUrl);
            _purchasesRequests = new PurchasesRequests(AppConfig.ApiUrl);
            _productsRequests = new ApiService(AppConfig.ApiUrl);
            _customerRequests = new CustomersRequests(AppConfig.ApiUrl);
            _supplierRequests = new SupplierRequests(AppConfig.ApiUrl);

            LogoutCommand = new ButtonCommand(Logout);

            // Initialize Labels with default values
            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };

            // Initialize BarSeries with default values
            BarSeries = new SeriesCollection
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
            LineSeries = new SeriesCollection
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

            CercleSeries = new SeriesCollection
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
            Task.Run(async () => await LoadDashboardDataAsync());
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
                if (BarSeries != null && BarSeries.Count >= 2)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        BarSeries[0].Values = salesCountValues;
                        BarSeries[1].Values = purchasesCountValues;
                    });
                }

                if (LineSeries != null && LineSeries.Count >= 2)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        LineSeries[0].Values = salesValues;
                        LineSeries[1].Values = purchasesValues;
                    });
                }

                if (CercleSeries != null && CercleSeries.Count >= 2)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        CercleSeries[0].Values = salesValues;

                        CercleSeries[1].Values = purchasesValues;
                        CercleSeries[2].Values = margeBenificiaire;

                    });
                }

                // Notify property changed
                OnPropertyChanged(nameof(Labels));
                OnPropertyChanged(nameof(BarSeries));
                OnPropertyChanged(nameof(LineSeries));
                OnPropertyChanged(nameof(CercleSeries));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating charts: {ex.Message}");
            }
        }

        //Using BarSeries for DashBoard
        public SeriesCollection BarSeries { get; set; }
        //Using PieSeries for Dashboard
        public SeriesCollection CercleSeries { get; set; }
        //Using LinesSeries For DashBoard
        public SeriesCollection LineSeries { get; set; }
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