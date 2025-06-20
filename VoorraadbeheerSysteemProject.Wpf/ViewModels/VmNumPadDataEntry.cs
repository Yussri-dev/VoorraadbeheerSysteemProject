﻿using System;
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
using VoorraadbeheerSysteemProject.Wpf.Services.Purchases;
using VoorraadbeheerSysteemProject.Wpf.Commands.PurchasesCommands;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmNumPadDataEntry : VmBase
    {
        #region Fields & Constructors
        private readonly VmSale _vmSale;
        private readonly VmPurchase _vmPurchase;
        private readonly SalesRequests _salesRequests;
        private readonly PurchasesRequests _purchasesRequests;

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
        //Input Text
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

        //Constructors
        public VmNumPadDataEntry(VmSale vmSale)
        {
            _vmSale = vmSale;

            AddSelectedProductCommand = new AddSelectedProductCommand(_vmSale, this);
            AddSaleAmountCommand = new AddSaleAmountCommand(this);
            SelectedAmounts = new ObservableCollection<SaleSelectedAmountRequest>();

            _salesRequests = new SalesRequests(AppConfig.ApiUrl);

            SelectedAmounts.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(MessageMontant));
            };

            InitialCommandsSales();
        }

        public VmNumPadDataEntry(VmPurchase vmPurchase)
        {
            _vmPurchase = vmPurchase;

            AddSelectedProductCommand = new AddSelectedProductToPurchaseCommand(_vmPurchase, this);

            _purchasesRequests = new PurchasesRequests(AppConfig.ApiUrl);

            InitialCommandsPurchases();
        }


        #endregion

        #region ObservableCollection
        public ObservableCollection<SaleSelectedAmountRequest> SelectedAmounts { get; set; }
        #endregion

        #region Methods
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

        //Calculate the Amount of Money inserted
        public string MessageMontant
        {
            get
            {
                decimal sommePayee = SelectedAmounts.Sum(s => s.AmountPrice);
                decimal total = _vmSale.TotalAmount;
                decimal difference = sommePayee - total;

                if (difference >= 0)
                {
                    return $"Change to return: {difference.ToString("C")}";
                }
                else
                {
                    return $"Amount remaining: {Math.Abs(difference).ToString("C")}";
                }
            }
        }

        private void InitialCommand()
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
            CloseWindowCommand = new ClosingCommand();
        }
        private void InitialCommandsSales()
        {
            InitialCommand();
            ValidateSaleDataCommand = new ValidateSaleDataCommand(_vmSale, this);
            RemoveSelectedSaleAmountCommand = new RemoveSelectedSaleAmountCommand(this);
            ClearSaleAmountCommand = new ClearSaleAmountCommand(this);
        }

        private void InitialCommandsPurchases()
        {
            InitialCommand();
            ValidatePurchaseDataCommand = new ValidatePurchaseDataCommand(_vmPurchase);

        }
        #endregion

        #region Commands Linked To Buttons
        public string FormattedTotalAmount => _vmSale.TotalAmount.ToString("C");
        public string FormattedCountLine => _vmSale.LineCount.ToString("C");
        public string FormattedTotalQuantity => _vmSale.TotalQuantity.ToString("C");
        #endregion

        #region Api Requests
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
        public async Task<SaleItemDTO?> SaveSaleItemAsync(SaleItemDTO saleItemDto)
        {
            try
            {
                var createdSale = await _salesRequests.PostSaleItemAsync(saleItemDto);

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
        #endregion

        #region commands
        public ICommand ClearSaleAmountCommand { get; set; }
        public ICommand RemoveSelectedSaleAmountCommand { get; set; }
        public ICommand ValidateSaleDataCommand { get; set; }
        public ICommand ValidatePurchaseDataCommand { get; set; }
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
        public ICommand CloseWindowCommand { get; set; }

        #endregion

    }
}
