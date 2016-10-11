using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class ChatToUser {

		[Key][Column(Order = 1)]
		[ForeignKey("ChatAppointment")]
		public int ChatID {
			get; set;
		}

		[Key]
		[Column(Order = 2)]
		[ForeignKey("MyUser")]
		public int UserID {
			get; set;
		}

		public virtual ChatAppointment ChatAppointment {
			get; set;
		}

		public virtual MyUser MyUser {
			get; set;
		}


	}
}