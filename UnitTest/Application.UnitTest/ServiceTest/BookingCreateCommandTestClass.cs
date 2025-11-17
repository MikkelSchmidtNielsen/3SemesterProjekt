using Application.ApplicationDto.Command;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    internal class BookingCreateCommandTestClass
    {
        new public IResult<CreatedBookingDto> Error<T>(CreatedBookingDto dto, IResult<T> errorResult)
        {
            return Error(dto, errorResult);
        }

        new public async Task<IResult<Guest>> CreateGuestAsync(CreatedBookingDto dto, BookingCreateDto input)
        {
            return await CreateGuestAsync(dto, input);
        }

        new public void AddPriceToDto(CreatedBookingDto dto, Resource resource)
        {
            AddPriceToDto(dto, resource);
        }
    }
}
