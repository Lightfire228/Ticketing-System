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

        // GET: Tickets
        public ActionResult Index()
        {

			string name = User.Identity.Name;

			// MyUser hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}
			
			// Still no idea what lambda expressions are, but it works
			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);
	
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

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.CUSTOMER)
				return RedirectToAction("Oops");

			List<Ticket> tickets = _dbContext.Tickets.ToList();
			List<UsersTickets> utRelation = _dbContext.UsersTickets.ToList();

			IEnumerable<Ticket> ticketQuery = 
				from ticket in tickets
				from ut in utRelation
				where ticket.ID == ut.TicketID && ut.UserID == user.ID
				select ticket;

			return View("Index", ticketQuery.ToList());

		}

		public ActionResult EmployeeIndex() {
			string name = User.Identity.Name;

			if (name == "") {
				return RedirectToAction("Oops");
			}

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.EMPLOYEE)
				return RedirectToAction("Oops");

			List<Ticket> tickets = _dbContext.Tickets.ToList();

			IEnumerable<Ticket> ticketQuery =
				from ticket in tickets
				where ticket.Status != TicketStatus.CLOSED
				select ticket;
			
			return View("Index", ticketQuery.ToList());
		}

		public ActionResult New() {

			string name = User.Identity.Name;

			// MyUser hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.CUSTOMER)
				return RedirectToAction("Oops");

			return View();
		}

		public ActionResult Add(NewTicketViewModel newTicket) {

			string name = HttpContext.User.Identity.GetUserName();

			if (name == "") {
				return RedirectToAction("Oops");
			}

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			if (user.Type != UserType.CUSTOMER)
				return RedirectToAction("Oops");

			Ticket ticket = new Ticket();
			TicketComponent component = new TicketComponent();
			UsersTickets usersTickets = new UsersTickets();

			ticket.Title = newTicket.Title;
			ticket.Time = DateTime.Now;
			ticket.Status = TicketStatus.OPENED;

			usersTickets.Ticket = ticket;
			usersTickets.MyUser = user;

			component.Ticket = ticket;
			component.MyUser = user;

			component.Text = newTicket.BodyText;
			component.Time = ticket.Time;

			_dbContext.Tickets.Add(ticket);
			_dbContext.UsersTickets.Add(usersTickets);
			_dbContext.TicketComponents.Add(component);
			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult View(Ticket ticket) {
			string name = HttpContext.User.Identity.GetUserName();

			if (name == "") {
				return RedirectToAction("Oops");
			}

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			List<TicketComponent> components = _dbContext.TicketComponents.ToList();

			IEnumerable<TicketComponent> componentQuery = 
				from component in components
				where component.TicketID == ticket.ID
				select component;

			EditTicketViewModel model = new EditTicketViewModel();

			model.Components = componentQuery.ToList();

			TicketComponent comp = new TicketComponent();
			model.ComponentToAdd = comp;
			model.TicketID = ticket.ID;

			switch (user.Type) {
				case UserType.CUSTOMER:
					if (ticket.Status == TicketStatus.CLOSED)
						return View("ClosedTicketsView", model);

					else 
						return View("CustomerView", model);

				case UserType.EMPLOYEE:
					return View("EmployeeView", model);
			}

			return RedirectToAction("Oops");
		}

		[HttpPost]
		public ActionResult EditTicket(EditTicketViewModel model) {
			string name = HttpContext.User.Identity.GetUserName();

			if (name == "") {
				return RedirectToAction("Oops");
			}

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			TicketComponent component = model.ComponentToAdd;

			component.Time = DateTime.Now;
			component.MyUser = user;
			component.Ticket = _dbContext.Tickets.Find(model.TicketID);
			component.Ticket.Status = model.Status;

			if (component.Ticket.Status == TicketStatus.CLOSED && user.Type == UserType.CUSTOMER)
				return RedirectToAction("Oops");

			_dbContext.TicketComponents.Add(component);
			_dbContext.SaveChanges();

			if (component.Ticket.Status == TicketStatus.CLOSED)
				return RedirectToAction("Index");

			return RedirectToAction("View", component.Ticket);
		}

		public ActionResult Report() {
			string name = HttpContext.User.Identity.GetUserName();

			if (name == "") {
				return RedirectToAction("Oops");
			}

			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			if (user.Type == UserType.CUSTOMER)
				return RedirectToAction("Oops");

			int totalTickets = 0;
			int closedTickets = 0;

			IEnumerable<Ticket> tickets = _dbContext.Tickets.ToList();

			foreach (Ticket ticket in tickets) {
				totalTickets++;
				if (ticket.Status == TicketStatus.CLOSED)
					closedTickets++;
			}

			ReportViewModel model = new ReportViewModel();

			model.Tickets = totalTickets;
			model.Closed = closedTickets;

			return View(model);
		}

		public ActionResult Oops() {
			return View();
		}
	}
}