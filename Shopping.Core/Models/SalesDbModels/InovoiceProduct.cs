using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class InovoiceProduct:BaseModel
    {
        public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
