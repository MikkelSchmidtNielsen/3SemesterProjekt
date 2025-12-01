using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
	public class ReadResourceByIdQueryHandler : IReadResourceByIdQueryHandler
	{
		private readonly IResourceRepository _resourceRepository;

		private ReadResourceByIdQueryHandler() // For unittest
		{
		}

		public ReadResourceByIdQueryHandler(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		public async Task<IResult<ResourceResponseDto>> HandleAsync(int id)
		{
			IResult<Resource?> result = await _resourceRepository.GetByIdAsync(id);

			// If response from database is null throw error
			Resource? resource = result.IsSucces() ? result.GetSuccess().OriginalType : null;

			if(resource is null)
			{
				throw new NotFoundException("Der er ikke nogen Resources i databasen med det Id");
			}

			// else return as success
			return Result<ResourceResponseDto>.Success(Mapper.Map<ResourceResponseDto>(resource));
		}
	}
}
