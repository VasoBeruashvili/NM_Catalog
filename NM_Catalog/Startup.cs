using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NM_Catalog.Startup))]
namespace NM_Catalog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
