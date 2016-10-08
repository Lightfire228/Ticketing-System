using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class Ticket {
	
		public int ID {
			get; set;
		}

		[Required]
		public string Title {
			get; set;
		}

		[Required]
		public DateTime Time {
			get; set;
		}

		[Required]
		public TicketStatus Status {
			get; set;
		}



	}

	public enum TicketStatus {
		OPENED,
		WORKED_ON,
		CLOSED
	}
}