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
        public string createAction;


        public NavigationModel(string name, string page, string permission)
        {
            this.name = name;
            this.page = page;
            this.permission = permission;
            this.createAction = "Edit";
        }

        public NavigationModel(string name, string page, string permission, string createAction)
        {
            this.name = name;
            this.page = page;
            this.permission = permission;
            this.createAction = createAction;
        }
    }
}