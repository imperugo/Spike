using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(imperugo.javascriptResult.ClientSample.Startup))]
namespace imperugo.javascriptResult.ClientSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
