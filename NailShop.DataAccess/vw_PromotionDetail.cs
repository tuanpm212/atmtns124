//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NailShop.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_PromotionDetail
    {
        public long PromotionID { get; set; }
        public int StoreID { get; set; }
        public int SiteID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public decimal Percent { get; set; }
        public decimal Quantity { get; set; }
        public int Sort { get; set; }
        public bool IsActive { get; set; }
        public bool IsItem { get; set; }
    }
}
