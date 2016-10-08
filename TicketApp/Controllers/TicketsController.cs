using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketApp.Models;

namespace TicketApp.Controllers
{
    public class TicketsController : Controller
    {

		private ApplicationDbContext _dbContext;

		public TicketsController() {
			_dbContext = new ApplicationDbContext();
		}

		/*
		 * Separate this into two actions : customer and employee
		 * Redirect the page if the cutomer somehow gets to the employee page
		 */ 
        // GET: Tickets
        public ActionResult Index()
        {

			var tickets = _dbContext.Tickets.ToList();
            return View(tickets);
        }

		public ActionResult New() {
			return View();
		}
    }
}