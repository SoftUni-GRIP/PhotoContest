using Microsoft.Owin;
using PhotoContest.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace PhotoContest.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}