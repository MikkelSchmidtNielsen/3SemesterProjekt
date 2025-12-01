using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Refit;

namespace Infrastructure.InternalApiCalls.ResourceApi
{
    public interface IResourceApi
    {
        /// <summary>
        /// Creates a resource.
        /// </summary>
        /// <param name="dto">The resource data to be sent in the JSON body</param>
        /// <returns>Api response</returns>
        [Post("/resources")]
        Task<CreateResourceByApiResponseDto> RegisterUserAsync([Body] CreateResourceCommandDto dto);
    }
}
