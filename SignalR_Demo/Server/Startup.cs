//
// http://designbased.net
//
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace Server
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(HubDemo), () => new HubDemo(new MessageRepository()));

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}