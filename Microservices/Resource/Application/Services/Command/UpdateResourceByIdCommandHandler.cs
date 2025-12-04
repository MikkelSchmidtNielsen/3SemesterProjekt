using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Domain.DomainDtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
	public class UpdateResourceByIdCommandHandler : IUpdateResourceByIdCommandHandler
	{
		private readonly IResourceRepository _resourceRepository;

		private UpdateResourceByIdCommandHandler() // Used for UnitTest
		{
		}

		public UpdateResourceByIdCommandHandler(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		public async Task<IResult<ResourceResponseDto>> HandleAsync(int id, UpdateResourceByIdCommandDto dto)
		{
			// Load it
			IResult<Resource?> getResponse = await _resourceRepository.GetByIdAsync(id);
			if (getResponse.IsSucces() is false ||
				(getResponse.IsSucces() && getResponse.GetSuccess().OriginalType is null))
			{
				throw new NotFoundException("Der er ikke nogen Resources i databasen med det Id");
			}

			Resource model = getResponse.GetSuccess().OriginalType!;

			// Do it
			model.UpdateResource(Mapper.Map<UpdateResourceDomainDto>(dto));

			// Save it
			await _resourceRepository.UpdateAsync(model);

			return Result<ResourceResponseDto>.Success(Mapper.Map<ResourceResponseDto>(model));
		}
	}
}
