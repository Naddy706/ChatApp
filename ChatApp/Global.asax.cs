using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ChatApp.Hubs;

namespace ChatApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           SqlDependency.Start(ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString);

        }

        protected void Session_Start(object sender,EventArgs args) {


            NotificationComponents NC = new NotificationComponents();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            NC.RegisterNotification(currentTime);

        }

        protected void Application_End() {
            SqlDependency.Stop(ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString);
        }
    }
}
