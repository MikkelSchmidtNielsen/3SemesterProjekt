using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Resource
    {
        public int Id { get; init; }
        public int bookingId { get; set; }
        public string Type { get; set; }

        // Entity Framework
        public Booking Booking { get; set; }

        public Resource(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
