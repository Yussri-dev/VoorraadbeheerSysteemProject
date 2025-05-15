using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int ProductUnitId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public string PackUnitType { get; set; } = string.Empty;
        public decimal QuantityStock { get; set; }
        public decimal QuantityPack { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; } = 0;
        public decimal SalePrice1 { get; set; } = 0;
        public decimal SalePrice2 { get; set; } = 0;
        public decimal SalePrice3 { get; set; } = 0;
        public string ImageUrl { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime DateModified { get; set; } = DateTime.Now;
        public bool IsActivate { get; set; } = true;
        public decimal DiscountPercentage { get; set; } = decimal.Zero;
        public bool IsSecondItemDiscountEligible { get; set; } 
        public bool IsBuyThreeForFiveEligible { get; set; }

        public int ShelfId { get; set; }
        public string ShelfName { get; set; } = string.Empty;
        public ShelfDTO Shelf { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public CategoryDTO? Category { get; set; }

        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;

        public int TaxId { get; set; }
        public decimal TaxRate { get; set; }
        public TaxDTO? Tax { get; set; }

        public int LineId { get; set; }
        public string LineName { get; set; } = string.Empty;
    }
}
