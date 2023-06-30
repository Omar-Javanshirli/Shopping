using Shopping.Core.Models.SalesDbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Dtos.Invoices
{
    public class InvoiceUpdateDto
    {
        public Invoice Invoice { get; set; }
        public IEnumerable<UpdateInvoiceProduct> Products { get; set; }
    }
}
