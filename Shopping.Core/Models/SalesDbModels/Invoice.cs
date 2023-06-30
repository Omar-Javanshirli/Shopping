using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class Invoice : BaseModel
    {
        public decimal TotalAmount { get; set; }
        public Guid CashierId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
