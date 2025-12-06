using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
    public class BookingFactory : IBookingFactory
    {
        public IResult<Booking> Create(CreateBookingFactoryDto dto)
        {
            try
            {
                Booking booking = new Booking(
                    dto.GuestId,
                    dto.ResourceId,
                    dto.StartDate,
                    dto.EndDate,
                    dto.TotalPrice);
                return Result<Booking>.Success(booking);
            }
            catch (Exception ex)
            {
                return Result<Booking>.Error(null, ex);
            }
        }
    }
}

