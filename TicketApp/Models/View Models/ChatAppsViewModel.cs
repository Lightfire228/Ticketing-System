using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class ChatAppsViewModel {

		public ChatAppsViewModel(Chat c, MyUser u) {
			Chat = c;			
			User = u;
		}

		public MyUser User {
			get; set;
		}

		public Chat Chat {
			get; set;
		}

		public DateTime Time {
			get; set;
		}
	}
}