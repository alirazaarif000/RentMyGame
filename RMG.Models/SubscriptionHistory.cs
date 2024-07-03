﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
	public class SubscriptionHistory
	{
		public int Id { get; set; }
		public string ApplicationUserId { get; set; }
		public int SubscriptionId { get; set; }
		public DateTime SubscribedDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int RemainingMonths { get; set; }
		public int NoOfMonths { get; set; }
		public double PricePaid { get; set; }
		public string Status { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
		[ForeignKey(nameof(SubscriptionId))]
		[ValidateNever]
		public Subscription Subscription { get; set; }
	}
}
