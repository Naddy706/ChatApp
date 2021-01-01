using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatApp.Startup))]
namespace ChatApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
