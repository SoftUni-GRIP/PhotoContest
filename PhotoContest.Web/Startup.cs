using Microsoft.Owin;
using Owin;
using PhotoContest.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace PhotoContest.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
