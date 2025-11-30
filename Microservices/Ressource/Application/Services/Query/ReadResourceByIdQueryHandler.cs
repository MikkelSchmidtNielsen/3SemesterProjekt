using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
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

		public ReadResourceByIdQueryHandler(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		public async Task<IResult<ResourceResponseDto>> HandleAsync(int id)
		{
			var result = await _resourceRepository.GetResourceByIdAsync(id);

			// Returns Dto format based on repository result
			return result.IsSucces() ?
				Result<ResourceResponseDto>.Success(Mapper.Map<ResourceResponseDto>(result.GetSuccess().OriginalType)).SetStatusCode(result.StatusCode)
				: Result<ResourceResponseDto>.Error(Mapper.Map<ResourceResponseDto>(new ResourceResponseDto()), result.GetError().Exception!).SetStatusCode(result.StatusCode);
		}
	}
}
