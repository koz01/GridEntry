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
    
    public partial class Pur_Inv_His
    {
        public Pur_Inv_His()
        {
            this.Pur_Detail_His = new HashSet<Pur_Detail_His>();
        }
    
        public int PurID { get; set; }
        public string VouNo { get; set; }
        public Nullable<System.DateTime> PurDate { get; set; }
        public string Remark { get; set; }
        public string SupplierName { get; set; }
    
        public virtual ICollection<Pur_Detail_His> Pur_Detail_His { get; set; }
    }
}
