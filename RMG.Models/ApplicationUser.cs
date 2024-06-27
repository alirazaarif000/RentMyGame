using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RMG.Models
{
	public class ApplicationUser:IdentityUser
	{
		public int? SubscriptionId { get; set; }
		[ForeignKey(nameof(SubscriptionId))]
		[ValidateNever]
		public Subscription Subscription { get; set; }
		[NotMapped]
		public string Role {  get; set; }

	}
}
