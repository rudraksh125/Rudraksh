using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Try_It___Twitter_Service.Startup))]
namespace Try_It___Twitter_Service
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
