using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketApp.Controllers
{
    public class ChatsController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
    }
}