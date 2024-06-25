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
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateOnly ReleaseDate { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        public int Stock {  get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public int GenreId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; }

        public int PlatformId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; }

        [NotMapped]
        public int Ratings { get; set; }
        [NotMapped]
        public int RatingCount { get; set; }
    }
}
