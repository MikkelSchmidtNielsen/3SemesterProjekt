using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainInterfaces
{
	public interface IBookingFactory
	{
		IResult<Booking> Create(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice);
    }
}
