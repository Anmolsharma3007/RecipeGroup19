using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Group19.Recipes1.Startup))]
namespace Group19.Recipes1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
