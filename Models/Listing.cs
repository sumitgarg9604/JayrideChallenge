using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jayride_Challenge.Models
{
    public class Quote
    {
        public string from { get; set; }

        public string to { get; set; }

        public Listing[] listings { get; set; }
    }

    public class Listing
    {
        public string name { get; set; }

        public float pricePerPassenger { get; set; }
        
        public Vehicle vehicleType { get; set; }

        public double totalPrice { get; set; }
    }

    
}
