using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPersistence;

namespace TODTool
{
    [Entity]
    public class Employee
    {
        [Id]
        public virtual string ID { get; set; }
        [Basic]
        public virtual string FirstName { get; set; }
        [Basic]
        public virtual string LastName { get; set; }
    }
}