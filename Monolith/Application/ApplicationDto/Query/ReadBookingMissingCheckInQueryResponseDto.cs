using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto.Query
{
    public class ReadBookingMissingCheckInQueryResponseDto
    {
        public int BookingId { get; set; }
        public string ResourceName { get; set; }
        public int ResourceLocation { get; set; }
        public DateOnly BookingStartDate {  get; set; }
        public DateOnly BookingEndDate { get; set; }
        public string GuestName { get; set; }
    }
}
