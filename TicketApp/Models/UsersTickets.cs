using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class UsersTickets {
		
		[Key]
		[ForeignKey("User")]
		public int UserID {
			get; set;
		}

		[Key]
		[ForeignKey("User")]
		public int TicketID {
			get; set;
		}


	}
}