//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TODTool
{
    using System;
    using System.Collections.Generic;
    
    public partial class FW_CALENDAR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FW_CALENDAR()
        {
            this.CE_DATA = new HashSet<CE_DATA>();
            this.OMNITURE_DATA = new HashSet<OMNITURE_DATA>();
            this.REPORT_DATA = new HashSet<REPORT_DATA>();
            this.TOD_DATA = new HashSet<TOD_DATA>();
        }
    
        public int ID { get; set; }
        public string Fiscal_Week { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CE_DATA> CE_DATA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OMNITURE_DATA> OMNITURE_DATA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPORT_DATA> REPORT_DATA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TOD_DATA> TOD_DATA { get; set; }
    }
}