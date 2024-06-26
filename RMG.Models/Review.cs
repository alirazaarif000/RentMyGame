using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
	public class Review
	{
		public int Id { get; set; }
		public string ApplicationUserId { get; set; }
		public int GameId { get; set; }
		public int Rating { get; set; }
		public string Subject { get; set; }
		public string Comment { get; set; }
		public DateOnly ReviewDate { get; set; }
		public bool IsApproved { get; set; }
		public string Status { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }

		[ForeignKey(nameof(GameId))]
		[ValidateNever]
		public Game Game { get; set; }


	}
}
