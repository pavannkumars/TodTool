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
    
    public partial class REPORT_DATA
    {
        public int REP_ID { get; set; }
        public int REP_ID_FK { get; set; }
        public string FW_ID { get; set; }
        public string REP_TYPE { get; set; }
        public byte[] REP_FILE { get; set; }
        public string REP_FILE_TYPE { get; set; }
        public string STATUS { get; set; }
    
        public virtual FW_CALENDAR FW_CALENDAR { get; set; }
    }
}
