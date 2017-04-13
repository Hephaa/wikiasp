using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wikiasp.Startup))]
namespace wikiasp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
