using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebStore_MVC.Infrastructure.Conventions
{
    public class TestControllersConvention:IControllerModelConvention
    {
        public TestControllersConvention()
        {
        }

        public void Apply(ControllerModel controller)
        {
            //controller.Actions.Add(new ActionModel());
            //throw new NotImplementedException();
        }
    }
}
