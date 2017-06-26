using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApi.Models;

namespace WebApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // FIXME
        public static List<MessageEntity> Messages = new List<MessageEntity>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
