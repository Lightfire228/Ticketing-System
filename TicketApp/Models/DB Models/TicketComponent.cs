using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class TicketComponent {

		public int ID {
			get; set;
		}

		[Required]
		public DateTime Time {
			get; set;
		}

		[ForeignKey("Ticket")]
		public int TicketID {
			get; set;
		}

		[ForeignKey("User")]
		public int UserID {
			get; set;
		}

		[Required]
		public string Text {
			get; set;
		}

		public virtual User User {
			get; set;
		}

		public virtual Ticket Ticket {
			get; set;
		}
		
	}
}