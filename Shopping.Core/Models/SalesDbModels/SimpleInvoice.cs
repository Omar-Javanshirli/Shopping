using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class SimpleInvoice
    {
        public Guid InvoiceId { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreationDate { get; set; }
        public string CashierFullName { get; set; }
        public string CustomerFullName { get; set; }
    }
}
