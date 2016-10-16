using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class NewChatViewModel {

		public List<MyUser> Users {
			get; set;
		}

		public int SelectedUserID {
			get; set;
		}

		public DateTime Time {
			get; set;
		}

	}
}