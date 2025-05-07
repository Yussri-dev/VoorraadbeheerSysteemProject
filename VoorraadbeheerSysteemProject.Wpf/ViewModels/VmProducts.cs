using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmProducts : VmBase
    {
        public VmProducts(NavigationStore navigationStore)
        {

            //NavigateDataCommand = new NavigationCommand<vmLogin>(navigationStore,
            //    () => new vmLogin(navigationStore));
            Products = new List<Product>()
            {
                new Product()
                {
                    Number = 1,
                    Name = "Product 1",
                    Purchase = 10.0,
                    Sale1 = 15.0,
                    Sale2 = 20.0,
                    TaxRate = "21%",
                    Category = "Category 1",
                    Barcode = "1234567890123"
                },
                new Product()
                {
                    Number = 2,
                    Name = "Product 2",
                    Purchase = 20.0,
                    Sale1 = 25.0,
                    Sale2 = 30.0,
                    TaxRate = "6%",
                    Category = "Category 2",
                    Barcode = "2345678901234"
                },
                new Product()
                {
                    Number = 3,
                    Name = "Product 3",
                    Purchase = 30.0,
                    Sale1 = 35.0,
                    Sale2 = 40.0,
                    TaxRate = "21%",
                    Category = "Category 3",
                    Barcode = "3456789012345"
                },
                new Product()
                {
                    Number = 4,
                    Name = "Product 4",
                    Purchase = 40.0,
                    Sale1 = 45.0,
                    Sale2 = 50.0,
                    TaxRate = "6%",
                    Category = "Category 4",
                    Barcode = "4567890123456"
                }
            };
        }

        public IList<Product> Products { get; set; }


    }

    //temp class
    public class Product
    {
        public int Number { get; set; }
        public string? Name { get; set; }
        public double Purchase { get; set; }
        public double Sale1 { get; set; }
        public double Sale2 { get; set; }
        public string? TaxRate { get; set; }
        public string? Category { get; set; }
        public string? Barcode { get; set; }
    }


}
