using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MapPlotter.UI.Web.Startup))]
namespace MapPlotter.UI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
