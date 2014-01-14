using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(imperugo.JavascriptResult.Startup))]
namespace imperugo.JavascriptResult
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
