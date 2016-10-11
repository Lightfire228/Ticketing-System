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
		private static int id;

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
			comp.Ticket = ticket;

			TicketsController.id = ticket.ID;

			// BECAUSE IT RESETS FOR SOME REASON
			ticket = _dbContext.Tickets.Find(id);

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
			// I REALIZE THIS IS BAD DESIGN, BUT MODEL.TICKET GETS OVERRIDEN
			// IN THE CALL TO THE VIEW, AND I CAN'T FIX THAT
			Ticket ticket = _dbContext.Tickets.Find(TicketsController.id);
			ticket.Status = model.Status;

			if (ticket.Status == TicketStatus.CLOSED && user.Type == UserType.CUSTOMER)
				return RedirectToAction("Oops");

			component.Ticket = ticket;

			_dbContext.TicketComponents.Add(component);
			_dbContext.SaveChanges();

			if (ticket.Status == TicketStatus.CLOSED)
				return RedirectToAction("Index");

			return RedirectToAction("View", component.Ticket);
		}

		public ActionResult Oops() {
			return View();
		}
	}
}