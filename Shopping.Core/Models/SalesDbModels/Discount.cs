using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class Discount:BaseModel
    {
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Percentage { get; set; }
    }
}
