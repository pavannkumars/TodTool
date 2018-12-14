using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace TODTool.Models
{
    public class CEDataModel
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<CE_DATA> CeDATAList { get; set; }

        public List<string> CeWkDataList { get; set; }

        public IEnumerable<SelectListItem> CeFwList {get; set;}

        

        public CEDataModel()
        {
            CeDATAList = new List<CE_DATA>();
            CeDATAList.Clear();
            CeWkDataList = new List<string>();
            CeWkDataList.Clear();
            CeFwList = new List<SelectListItem>();
        }


    }
}