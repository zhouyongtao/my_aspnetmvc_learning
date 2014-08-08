using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(my_aspnetmvc_learning.Startup))]
namespace my_aspnetmvc_learning
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
