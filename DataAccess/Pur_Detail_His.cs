//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pur_Detail_His
    {
        public int DetaiID { get; set; }
        public int PurID { get; set; }
        public int CardID { get; set; }
        public string Qty { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<int> SrNo { get; set; }
    
        public virtual Pur_Inv_His Pur_Inv_His { get; set; }
    }
}
