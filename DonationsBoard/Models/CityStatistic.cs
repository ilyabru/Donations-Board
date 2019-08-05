using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationBoard.Models
{
    public class CityStatistic
    {
        public string City { get; set; }
        public int TotalDonations { get; set; }
        public decimal AmountRaised { get; set; }
        public decimal AverageRaised { get; set; }
    }
}
