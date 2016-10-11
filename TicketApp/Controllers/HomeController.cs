using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketApp.Controllers {
	public class HomeController : Controller {
		public ActionResult Index() {
			return View();
		}

		public ActionResult About() {
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult PonyView() {
			return View();
		}

		public ActionResult MeGustaView() {
			return View();
		}

		public ActionResult WahjudiView() {
			return View();
		}

		public ActionResult Pi() {
			return View();
		}
	}
}