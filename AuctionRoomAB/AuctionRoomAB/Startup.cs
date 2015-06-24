using Microsoft.Owin;
using Owin;

namespace AuctionRoomAB
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            NHibernateHelper.Initialize();
            app.MapSignalR();
        }
    }
}