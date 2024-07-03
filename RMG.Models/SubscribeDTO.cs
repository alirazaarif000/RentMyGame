using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class SubscribeDTO
    {
        public int SubcriptionId { get; set; }
        public int NoOfMonths { get; set; }
        public double PricePaid { get; set; }
    }
}
