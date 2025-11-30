using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
	partial class ReadResourceWithCriteriaQueryHandler : IReadResourceWithCriteriaQueryHandler
	{
		private readonly IResourceRepository _resourceRepository;

		public ReadResourceWithCriteriaQueryHandler(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		public async Task<IResult<IEnumerable<ResourceResponseDto>>> HandleAsync(ReadResourceListQueryDto criteria)
		{
			IResult<IEnumerable<Resource>> result = await _resourceRepository.GetAllResourcesAsync(criteria);

			List<ResourceResponseDto> response = new List<ResourceResponseDto>();

			// Returns Dto format based on repository result
			if (result.IsSucces() is false)
			{
				foreach(var resource in result.GetError().OriginalType)
				{
					ResourceResponseDto resourceDto = Mapper.Map<ResourceResponseDto>(resource);
					resourceDto.StatusCode = HttpStatusCode.InternalServerError;

					response.Add(resourceDto);
				}

				return Result<IEnumerable<ResourceResponseDto>>.Error(response, result.GetError().Exception!).SetStatusCode(result.StatusCode);
			}
			else
			{
				foreach (var resource in result.GetSuccess().OriginalType)
				{
					ResourceResponseDto resourceDto = Mapper.Map<ResourceResponseDto>(resource);
					resourceDto.StatusCode = HttpStatusCode.OK;

					response.Add(resourceDto);
				}

				return Result<IEnumerable<ResourceResponseDto>>.Success(response).SetStatusCode(result.StatusCode);
			}	
		}
	}
}
