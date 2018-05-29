using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageServiceWeb1.Startup))]
namespace ImageServiceWeb1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
