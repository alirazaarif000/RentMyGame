using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
	public class Rental
	{
		[Key]
		public int Id { get; set; }
		public string ApplicationUserId { get; set; }
		[ForeignKey(nameof(ApplicationUserId))]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }	

		public int GameId { get; set; }
		[ForeignKey(nameof(GameId))]
		[ValidateNever]
		public Game Game { get; set; }
		public DateTime RentalDate { get; set; }
		public DateTime ReturnDate { get; set; }
		public string Status { get; set; }

	}
}
