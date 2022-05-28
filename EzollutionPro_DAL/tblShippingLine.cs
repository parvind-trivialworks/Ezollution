//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EzollutionPro_DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblShippingLine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblShippingLine()
        {
            this.tblBondMasterMs = new HashSet<tblBondMasterM>();
            this.tblSeaSchedulings = new HashSet<tblSeaScheduling>();
        }
    
        public int iShippingID { get; set; }
        public string sMLOCode { get; set; }
        public string sShippingLineName { get; set; }
        public string sDescription { get; set; }
        public Nullable<bool> bStatus { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dtCreatedDate { get; set; }
        public Nullable<int> iModifiedBy { get; set; }
        public Nullable<System.DateTime> dtModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBondMasterM> tblBondMasterMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSeaScheduling> tblSeaSchedulings { get; set; }
    }
}