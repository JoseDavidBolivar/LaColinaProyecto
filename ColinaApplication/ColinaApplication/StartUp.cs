using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ColinaApplication.Startup))]
namespace ColinaApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}