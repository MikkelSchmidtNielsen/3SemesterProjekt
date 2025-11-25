using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
	public interface IGuestFactory
	{
		IResult<Guest> Create(CreatedGuestDto dto);
	}
    public interface IGetAllResourcesService
    {
        Task<IResult<IEnumerable<Resource>>> GetAllResourcesAsync();
    }
}
