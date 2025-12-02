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
        Task<CreateResourceByApiResponseDto> CreateResourceAsync([Body] CreateResourceCommandDto dto);

        /// <summary>
        /// Reads a resource by id.
        /// </summary>
        /// <param name="id">The resource is recieved by id</param>
        /// <returns>One resource</returns>
        [Get("/resources/{id}")]
        Task<ReadResourceByIdByApiResponseDto> ReadResourceByIdAsync(int id);

        /// <summary>
        /// Reads a resource by id.
        /// </summary>
        /// <returns>List of resources</returns>
        [Get("/resources")]
        Task<ICollection<ReadAllResourceByApiResponse>> ReadAllResourcesAsync([Query] InternalResourceApiFilterDto filter);
    }
}
