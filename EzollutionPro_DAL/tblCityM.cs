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
    
    public partial class tblCityM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCityM()
        {
            this.tblUserMs = new HashSet<tblUserM>();
        }
    
        public int iCityId { get; set; }
        public int iStateId { get; set; }
        public string sCityName { get; set; }
        public string sDescription { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
    
        public virtual tblStateM tblStateM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUserM> tblUserMs { get; set; }
    }
}
