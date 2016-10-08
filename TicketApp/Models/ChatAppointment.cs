using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class ChatAppointment {

		public int ID {
			get; set;
		}

		[Required]
		public DateTime Time {
			get; set;
		}

	}
}