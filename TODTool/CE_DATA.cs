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
    
    public partial class CE_DATA
    {
        public string FW_ID { get; set; }
        public int CE_SEQUENCE_ID { get; set; }
        public System.DateTime REP_GEN_DATE { get; set; }
        public System.DateTime FW_START_DATE { get; set; }
        public System.DateTime FW_END_DATE { get; set; }
        public System.DateTime CREATED_DATE { get; set; }
        public System.DateTime CREATED_TIME { get; set; }
        public string PUBLICATION { get; set; }
        public string LANGUAGE { get; set; }
        public string GUID { get; set; }
        public int ACCURATE_RATING { get; set; }
        public int USEFUL_RATING { get; set; }
        public int EASY_TO_UNDERSTAND_RATING { get; set; }
        public string ARTICLE_SOLVED_ISSUE { get; set; }
        public string FEEDBACK { get; set; }
        public string CE_STATUS { get; set; }
        public string Title { get; set; }
    
        public virtual FW_CALENDAR FW_CALENDAR { get; set; }
    }
}
