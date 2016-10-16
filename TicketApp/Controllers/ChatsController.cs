using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketApp.Models;

namespace TicketApp.Controllers
{
    public class ChatsController : Controller
    {

		private Models.ApplicationDbContext _dbContext;

		public ChatsController () {
			_dbContext = new Models.ApplicationDbContext();
		}

		public ActionResult Index() {

			string name = User.Identity.Name;

			// MyUser hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}

			// Still no idea what lambda expressions are, but it works
			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			// Get all chats that exist that aren't my user type
			var chats = 
				from chat in _dbContext.Chats.ToList()
				from chatRelation in _dbContext.ChatToUsers.ToList()
				from newUser in _dbContext.MyUsers.ToList()

				where chat.ID == chatRelation.ChatID && 
					  chatRelation.UserID == newUser.ID &&
					  newUser.Type != user.Type
				select new {Chat = chat, User = newUser};

			// Get all chats from first query that are related to me
			chats = 
				from chat in chats
				from chatRelation in _dbContext.ChatToUsers.ToList()

				where chat.Chat.ID == chatRelation.ChatID &&
					  chatRelation.UserID == user.ID
				select chat;

			List<ChatAppsViewModel> model = new List<ChatAppsViewModel>();

			foreach (var chat in chats) {
				model.Add(new ChatAppsViewModel(chat.Chat, chat.User));
			}

			return View(model);
		}

		public ActionResult New() {

			string name = User.Identity.Name;

			// MyUser hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}

			// Still no idea what lambda expressions are, but it works
			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			IEnumerable<MyUser> users =
				from newUser in _dbContext.MyUsers.ToList()
				where newUser.Type != user.Type
				select newUser;

			NewChatViewModel model = new NewChatViewModel();

			model.Users = users.ToList();

			return View(model);
		}

		[HttpPost]
		public ActionResult Add(NewChatViewModel model) {

			string name = User.Identity.Name;

			// MyUser hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}

			// Still no idea what lambda expressions are, but it works
			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			Chat chat = new Chat();
			chat.Time = model.Time;

			ChatToUser userChat = new ChatToUser();
			ChatToUser newUserChat = new ChatToUser();

			userChat.Chat = chat;
			newUserChat.Chat = chat;

			userChat.User = user;
			newUserChat.User = _dbContext.MyUsers.Find(model.SelectedUserID);

			_dbContext.Chats.Add(chat);
			_dbContext.ChatToUsers.Add(userChat);
			_dbContext.ChatToUsers.Add(newUserChat);
			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id) {

			string name = User.Identity.Name;

			// MyUser hasn't logged in yet
			if (name == "") {
				return RedirectToAction("Oops");
			}

			// Still no idea what lambda expressions are, but it works
			MyUser user = _dbContext.MyUsers.SingleOrDefault(u => u.Email == name);

			Chat chat = _dbContext.Chats.Find(id);

			_dbContext.Chats.Remove(chat);
			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Oops() {
			return View();
		}
	}
}