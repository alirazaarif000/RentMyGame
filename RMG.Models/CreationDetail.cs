using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class CreationDetail
    {
        [ValidateNever]
        public DateTime CreatedDate { get; set; }
        [ValidateNever]
        public string CreatedBy { get; set; }
    }
}
