using System.Web.Mvc;
using NHibernate;
using AuctionRoomAB.Models;

namespace AuctionRoomAB.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult AuctionRoom()
        {
            ISession session = NHibernateHelper.CurrentSessionFactory.OpenSession();

            Item firstItem = session.QueryOver<Item>().SingleOrDefault();

            return View(firstItem);
        }
    }
}
