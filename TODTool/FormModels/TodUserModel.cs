using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TODTool.FormModels
{
    public class TodUserModel
    {
        
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is mandatory")]
        public string userRole { get; set; }

    }

    
}