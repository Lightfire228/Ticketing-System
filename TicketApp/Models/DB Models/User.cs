using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TicketApp.Models {
	public class User {
		
		[Required]
		public UserType Type {
			get; set;
		}

		[Required]
		public string Email {
			get; set;
		}

		public int ID {
			get; set;
		}

	}

	public enum UserType {
		CUSTOMER,
		EMPLOYEE
	}
}