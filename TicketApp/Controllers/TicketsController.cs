using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketApp.Models;
using Microsoft.AspNet.Identity;

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

			string name = User.Identity.Name;

			// User hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}
			
			// Still no idea what lambda expressions are, but it works
			User user = _dbContext.User.SingleOrDefault(u => u.Email == name);
	
			// Route to page for user
			switch (user.Type) {
				case UserType.CUSTOMER:
					return RedirectToAction("CustomerIndex");

				case UserType.EMPLOYEE:		
					return RedirectToAction("EmployeeIndex");

				default:
					return HttpNotFound();
			}
        }

		public ActionResult CustomerIndex() {

			string name = User.Identity.Name;

			if (name == "") {
				return RedirectToAction("Oops");
			}

			User user = _dbContext.User.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.CUSTOMER)
				return RedirectToAction("NoAccess");

			List<Ticket> tickets = _dbContext.Tickets.ToList();
			List<UsersTickets> utRelation = _dbContext.UsersTickets.ToList();

			// I JUST WANT SQL COMMANDS PLEASE, THIS IS SO MUCH HARDER THAN JUST SENDING A METHOD
			// A STRING OF SQL COMMANDS ASDFKLNASDF;LNHAS
			IEnumerable<Ticket> ticketQuery = 
				from ticket in tickets
				from ut in utRelation
				where ticket.ID == ut.TicketID && ut.UserID == user.ID
				select ticket;

			// Actually, it is easier, and safer against SQL injection, but it's hard to figure out what works
			return View(ticketQuery.ToList());
//			return View(tickets);

		}

		public ActionResult New() {

			string name = User.Identity.Name;

			// User hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}

			User user = _dbContext.User.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.CUSTOMER)
				RedirectToAction("NoAccess");

			return View();
		}

		public ActionResult Add(NewTicketViewModel newTicket) {

			string name = HttpContext.User.Identity.GetUserName();

			if (name == "") {
				return RedirectToAction("Oops");
			}

			User user = _dbContext.User.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.CUSTOMER)
				return RedirectToAction("NoAccess");

			Ticket ticket = new Ticket();
			TicketComponent component = new TicketComponent();
			UsersTickets usersTickets = new UsersTickets();

			ticket.Title = newTicket.Title;
			ticket.Time = DateTime.Now;
			ticket.Status = TicketStatus.OPENED;

			usersTickets.Ticket = ticket;
			usersTickets.User = user;

			component.Ticket = ticket;
			component.User = user;

			component.Text = newTicket.BodyText;
			component.Time = ticket.Time;

			_dbContext.Tickets.Add(ticket);
			_dbContext.UsersTickets.Add(usersTickets);
			_dbContext.TicketComponents.Add(component);
			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id) {
			var video = _dbContext.Tickets.SingleOrDefault(v => v.ID == id);

			if (video == null)
				return HttpNotFound();

			return View(video);
		}

		public ActionResult Oops() {
			return View();
		}
    }
}