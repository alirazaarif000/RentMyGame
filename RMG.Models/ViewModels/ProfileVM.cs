using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models.ViewModels
{
    public class ProfileVM
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
