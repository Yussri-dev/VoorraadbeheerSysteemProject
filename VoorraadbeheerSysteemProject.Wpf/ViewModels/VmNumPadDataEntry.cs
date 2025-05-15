using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Views;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Requests;
using VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands;
using VoorraadbeheerSysteemProject.Wpf.Services.Sales;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmNumPadDataEntry : VmBase
    {
        //private readonly NavigationStore _navigationStore;
        private readonly VmSale _vmSale;
        private readonly SalesRequests _salesRequests;

        public ObservableCollection<SaleSelectedAmountRequest> SelectedAmounts { get; set; }

        private SaleSelectedAmountRequest? _selectedAmount;
        public SaleSelectedAmountRequest? SelectedAmount
        {
            get => _selectedAmount;
            set
            {
                _selectedAmount = value;
                OnPropertyChanged(nameof(SelectedAmount));
            }
        }
        //public decimal CurrentAmountPrice { get; set; }

        public VmNumPadDataEntry(VmSale vmSale)
        {
            _vmSale = vmSale;

            AddSelectedProductCommand = new AddSelectedProductCommand(_vmSale, this);
            AddSaleAmountCommand = new AddSaleAmountCommand(this);
            SelectedAmounts = new ObservableCollection<SaleSelectedAmountRequest>();

            _salesRequests = new SalesRequests("https://localhost:5001/");

            SelectedAmounts.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(FormattedSelectedAmountTotal));
            };
            InitialCommands();
        }
        public decimal GetTotalAmount()
        {
            return _vmSale.TotalAmount;
        }

        public decimal GetTotalQuantity()
        {
            return _vmSale.TotalQuantity;
        }

        public int GetLineCount()
        {
            return _vmSale.LineCount;
        }
        public string FormattedTotalAmount => _vmSale.TotalAmount.ToString("C");
        public string FormattedCountLine => _vmSale.LineCount.ToString("C");
        public string FormattedTotalQuantity => _vmSale.TotalQuantity.ToString("C");

        private string _inputText = string.Empty;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        //Total Amount
        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
            }
        }


        // Total Quantity
        private decimal _totalQuantity;
        public decimal TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                _totalQuantity = value;
            }
        }

        //Total Lines
        private int _lineCount;
        public int LineCount
        {
            get => _lineCount;
            set
            {
                _lineCount = value;

            }
        }


        public string FormattedSelectedAmountTotal => SelectedAmounts.Sum(s => s.AmountPrice).ToString("C");

        private void InitialCommands()
        {
            Number0Command = new AppendNumberCommand(this, "0");
            Number00Command = new AppendNumberCommand(this, "00");
            Number1Command = new AppendNumberCommand(this, "1");
            Number2Command = new AppendNumberCommand(this, "2");
            Number3Command = new AppendNumberCommand(this, "3");
            Number4Command = new AppendNumberCommand(this, "4");
            Number5Command = new AppendNumberCommand(this, "5");
            Number6Command = new AppendNumberCommand(this, "6");
            Number7Command = new AppendNumberCommand(this, "7");
            Number8Command = new AppendNumberCommand(this, "8");
            Number9Command = new AppendNumberCommand(this, "9");
            NumberPuntCommand = new AppendNumberCommand(this, ".");

            DeleteCommand = new DeleteInputCommand(this);
            ReturnCommand = new ReturnInputCommand(this);

            ValidateSaleDataCommand = new ValidateSaleDataCommand(_vmSale,this);
            RemoveSelectedSaleAmountCommand = new RemoveSelectedSaleAmountCommand(this);

            ClearSaleAmountCommand = new ClearSaleAmountCommand(this);
            //AddSelectedProductCommand = new AddSelectedProductCommand(_vmSale,this);
        }

        public async Task<SaleDTO?> SaveSaleAsync(SaleDTO saleDto)
        {
            try
            {
                var createdSale = await _salesRequests.PostSaleAsync(saleDto);

                if (createdSale != null)
                {
                    return createdSale;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region commands
        public ICommand ClearSaleAmountCommand { get; set; }
        public ICommand RemoveSelectedSaleAmountCommand { get; set; }
        public ICommand ValidateSaleDataCommand { get; set; }
        public ICommand AddSaleAmountCommand { get; set; }
        public ICommand AddSelectedProductCommand { get; set; }
        public ICommand RemoveSelectedProductCommand { get; set; }
        public ICommand ClearSelectedProductCommand { get; set; }
        public ICommand OpenDialogCommand { get; set; }
        public ICommand SearchProductsCommand { get; set; }
        public ICommand Number0Command { get; set; }
        public ICommand Number00Command { get; set; }
        public ICommand Number1Command { get; set; }
        public ICommand Number2Command { get; set; }
        public ICommand Number3Command { get; set; }
        public ICommand Number4Command { get; set; }
        public ICommand Number5Command { get; set; }
        public ICommand Number6Command { get; set; }
        public ICommand Number7Command { get; set; }
        public ICommand Number8Command { get; set; }
        public ICommand Number9Command { get; set; }
        public ICommand NumberPuntCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        #endregion

    }
}
