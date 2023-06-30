using Shopping.Core.Models.SalesDbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Dtos.Invoices
{
    public class InvoiceCreateDto
    {
        public Invoice Invoice { get; set; }
        public IEnumerable<InvoiceProduct> Products { get; set; }
    }
}
