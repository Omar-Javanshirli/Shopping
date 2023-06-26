using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }
        public string Shopping { get; set; }

        public class ServiceApi
        {
            public string Path { get; set; }
        }
    }
}
