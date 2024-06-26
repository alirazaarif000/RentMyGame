using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models.ViewModels
{
	public class SubscriptionVM
	{
		public List<Subscription> Subscription {  get; set; }
		public int? SubscribedId { get; set; }
	}
}
