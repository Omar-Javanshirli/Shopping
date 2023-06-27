using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class Product:BaseModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid DiscountId { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
