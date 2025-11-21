using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto.Query
{
    public class BookingMissingCheckInQueryDto
    {
        public int BookingId { get; set; }
        public int ResourceId { get; set; }
        public int GuestId { get; set; }
    }
}
