using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.Models.SalesDbModels
{
    public class BaseModel
    {
        public Guid Id { get; set; } = new Guid();
        public bool DeleteStatus { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int RowNum { get; set; }
    }
}
