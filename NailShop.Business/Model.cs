using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public class InvoiceSyn
    {
        public List<Invoice> Invoice { get; set; }
        public List<OrdItem> OrdItem { get; set; }
    }
}
