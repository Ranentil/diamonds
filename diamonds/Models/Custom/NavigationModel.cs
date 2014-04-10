using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diamonds.Models
{
    public class NavigationModel
    {
        public string name;
        public string page;
        public string permission;


        public NavigationModel(string name, string page, string permission)
        {
            this.name = name;
            this.page = page;
            this.permission = permission;
        }
    }
}