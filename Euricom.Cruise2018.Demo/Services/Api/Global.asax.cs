using System.Web.Http;

namespace Euricom.Cruise2018.Demo.Services.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(AutofacConfig.Register);
        }
    }
}
