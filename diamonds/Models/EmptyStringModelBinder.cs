using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class EmptyStringModelBinder : DefaultModelBinder 
{
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
        return base.BindModel(controllerContext, bindingContext);
    }
}