using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class EditTicketViewModel {

		public List<TicketComponent> Components {
			get; set;
		}

		public TicketComponent ComponentToAdd {
			get; set;
		}

		public TicketStatus Status {
			get; set;
		}

		public Ticket Ticket {
			get; set;
		}
	}
}