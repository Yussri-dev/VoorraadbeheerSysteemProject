using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmInventory : VmBase
    {
        public IList<Purchase> Purchases { get; set; }

        public VmInventory(NavigationStore navigationStore)
        {

            //fill purchase list with dummy data
            Purchases = new List<Purchase>
            {
                new Purchase
                {
                    Id = 1,
                    ProductName = "Product 1",
                    Amount = 10,
                    SalePrice = 15.0,
                    TaxRate = "21%",
                    Supplier = "Supplier 1",
                    SaleDate = DateTime.Now,
                    Stock = 100
                },
                new Purchase
                {
                    Id = 2,
                    ProductName = "Product 2",
                    Amount = 20,
                    SalePrice = 25.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 200
                },
                new Purchase
                {
                    Id = 3,
                    ProductName = "Product 2",
                    Amount = 30,
                    SalePrice = 35.0,
                    TaxRate = "21%",
                    Supplier = "Supplier 3",
                    SaleDate = DateTime.Now,
                    Stock = 300
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400,
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
                new Purchase
                {
                    Id = 4,
                    ProductName = "Product 3",
                    Amount = 40,
                    SalePrice = 45.0,
                    TaxRate = "6%",
                    Supplier = "Supplier 2",
                    SaleDate = DateTime.Now,
                    Stock = 400
                },
            };
        }

        //temp purchase class
        public class Purchase
        {
            public int Id { get; set; }
            public string? ProductName { get; set; }
            public int Amount { get; set; }
            public double SalePrice { get; set; }
            public string? TaxRate { get; set; }
            public string? Supplier { get; set; }
            public DateTime SaleDate { get; set; }
            public int Stock { get; set; }

        }
    }
}


