using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business.Model
{
    public class ModelInvoice
    {
        public List<SynInvoice> listSynInvoice { get; set; }
        public List<SynOrderItem> listOrdItem { get; set; }
    }
}
