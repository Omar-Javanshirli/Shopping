using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class FullInvoiceDetails
    {
        public SimpleInvoice Invoice { get; set; }
        public IEnumerable<ProductInvoiceItem> Products { get; set; }
    }
}
