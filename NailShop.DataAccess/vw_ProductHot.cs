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
    
    public partial class vw_ProductHot
    {
        public long ProductID { get; set; }
        public string ProductCode { get; set; }
        public int StoreID { get; set; }
        public int SiteID { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LargeImage { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public Nullable<int> Sort { get; set; }
        public bool IsActive { get; set; }
    }
}
