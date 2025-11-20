using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelsDto
{
    public class BookingCreateFactoryDto
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int ResourceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
