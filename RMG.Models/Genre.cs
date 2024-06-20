using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Genre")]
        public string GenreName { get; set; }
    }
}
