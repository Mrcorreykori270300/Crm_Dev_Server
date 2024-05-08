using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Inwinteck_CRM.Startup))]
namespace Inwinteck_CRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        //public void Configuration(IAppBuilder app)
        //{
        //    app.MapSignalR();
        //}
    }
}
