using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PackageName { get; set; }
        [Required]
        public int RentCount { get; set; }
        [Required]
        public int NewGameCount { get; set; }
        [Required]
        public int ReplaceCount { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
