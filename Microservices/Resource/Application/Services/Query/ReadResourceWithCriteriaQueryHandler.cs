using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.CustomExceptions;
using Common.ExtensionMethods;
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
	public class ReadResourceWithCriteriaQueryHandler : IReadResourceWithCriteriaQueryHandler
	{
		private readonly IResourceRepository _resourceRepository;

		private ReadResourceWithCriteriaQueryHandler() // For UnitTest
		{
		}

		public ReadResourceWithCriteriaQueryHandler(IResourceRepository resourceRepository)
		{
			_resourceRepository = resourceRepository;
		}

		

		public async Task<IResult<ICollection<ResourceResponseDto>>> HandleAsync(ReadResourceListQueryDto criteria)
		{
			IResult<IEnumerable<Resource>> result = await _resourceRepository.GetAllAsync(criteria);

			List<ResourceResponseDto> reponses = new List<ResourceResponseDto>();

            // If it is not a success, then it will throw an exception which our ExceptionHandler manages/takes care of
            foreach (Resource resource in result.GetSuccess().OriginalType)
			{
				reponses.Add(Mapper.Map<ResourceResponseDto>(resource));
			}

			if (reponses.Count is 0)
			{
				throw new NotFoundException("Der er ikke nogen Resources i databasen med de kriteria");
			}
			
			return Result<ICollection<ResourceResponseDto>>.Success(reponses);
		}
	}
}
