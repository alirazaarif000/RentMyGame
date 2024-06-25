using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models.ViewModels
{
    public class RentGameVM
    {
        public Game Game { get; set; }
        public Rental Rental { get; set; }
        public Review Review { get; set; }
    }
}
