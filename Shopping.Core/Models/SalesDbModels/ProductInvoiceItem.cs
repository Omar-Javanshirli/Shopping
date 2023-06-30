using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class ProductInvoiceItem
    {
        public string ProductTitle { get; set; }
        public int ProductQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountedAmount { get; set; }
        public string DiscountDetails { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public int DiscountRate { get; set; }
    }
}
