using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business.Model.ModelWeb
{
    public class Home
    {
        public List<vw_Slide> Sliders { get; set; }
        public vw_Slide Welcome { get; set; }
        public List<vw_News> News { get; set; }
        public List<vw_Brand> Brands { get; set; }
        public List<Store> Stores { get; set; }
        public List<vw_ProductHot> ProductHots { get; set; }
    }

    public class LoginModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
    }
    
    public class ChangePassword
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordConfirm { get; set; }
    }

    public class ForgetPassword
    {
        public string Email { get; set; }
    }

    public class SearchInvoice
    {
        public int Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<vw_Invoice> Invoices { get; set; }
    }

    public class InvoiceModel
    {
        public Invoice Invoice { get; set; }
        public Customer Customer { get; set; }
    }
}
