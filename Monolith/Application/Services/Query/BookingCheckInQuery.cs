using Application.ApplicationDto.Query;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Query
{
    public class BookingCheckInQuery : IBookingCheckInQuery
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IReadAllResourcesQuery _resourcesQuery;
        private readonly ResourceFilterDto _filter = new ResourceFilterDto { IsAvailable = true };

        public BookingCheckInQuery(IBookingRepository bookingRepository, IReadAllResourcesQuery resourcesQuery)
        {
            _bookingRepository = bookingRepository;
            _resourcesQuery = resourcesQuery;
        }

        public async Task<IResult<List<ReadBookingMissingCheckInQueryResponseDto>>> GetActiveBookingsWithMissingCheckInsAsync()
        {
            // Creates tasks to run
            Task<IResult<IEnumerable<Booking>>> missingCheckInTask = _bookingRepository.GetActiveBookingsWithMissingCheckInsAsync();
            Task<IResult<IEnumerable<ReadResourceQueryResponseDto>>> resourcesTask = _resourcesQuery.ReadAllResourcesAsync(_filter);

            // Runs both task simultaneously, so they don't wait for each other
            await Task.WhenAll(missingCheckInTask, resourcesTask);

            // Get task results
            IResult<IEnumerable<Booking>> missingCheckinTaskResult = missingCheckInTask.Result;
            IResult<IEnumerable<ReadResourceQueryResponseDto>> resourcesTaskResult = resourcesTask.Result;

            // Failed to get bookings return error message
            if (missingCheckinTaskResult.IsSucces() == false)
            {
                return Result<List<ReadBookingMissingCheckInQueryResponseDto>>.Error(null!, new Exception("Kunne ikke hente bookinger fra databasen."));
            }

            // Failed to get resources return error message
            if (resourcesTaskResult.IsSucces() == false)
            {
                return Result<List<ReadBookingMissingCheckInQueryResponseDto>>.Error(null!, new Exception("Kunne ikke hente ressourcer fra databasen."));
            }

            // Get success results
            IEnumerable<Booking> missingCheckIns = missingCheckinTaskResult.GetSuccess().OriginalType;
            IEnumerable<ReadResourceQueryResponseDto> resources = resourcesTaskResult.GetSuccess().OriginalType;

            // Mapping the resources to a dictionary for instant look up
            Dictionary<int, ReadResourceQueryResponseDto> resourceMap = resources.ToDictionary(resource => resource.Id, resource => resource);

            // Creates the response list to return
            List<ReadBookingMissingCheckInQueryResponseDto> responseList = new List<ReadBookingMissingCheckInQueryResponseDto>();

            foreach (Booking booking in missingCheckIns) // Iterates through each booking and "converts" them into a dto.
            {
                // Gets value from dictionary
                if (resourceMap.TryGetValue(booking.ResourceId, out ReadResourceQueryResponseDto? matchingResource))
                {
                    ReadBookingMissingCheckInQueryResponseDto missingCheckInInfo = new ReadBookingMissingCheckInQueryResponseDto
                    {
                        BookingId = booking.Id,
                        ResourceName = matchingResource.Name,
                        ResourceLocation = matchingResource.Location,
                        BookingStartDate = booking.StartDate,
                        BookingEndDate = booking.EndDate,
                        GuestName = $"{booking.Guest.FirstName} {booking.Guest.LastName}"
                    };

                    responseList.Add(missingCheckInInfo);
                }
            }

            if (responseList.Any())
            {
                return Result<List<ReadBookingMissingCheckInQueryResponseDto>>.Success(responseList);
            }
            
            return Result<List<ReadBookingMissingCheckInQueryResponseDto>>.Error(responseList, new Exception("Der er ingen manglende indtjekninger."));
        }
    }
}
