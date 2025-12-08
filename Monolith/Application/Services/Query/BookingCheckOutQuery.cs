using Application.ApplicationDto.Query;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;

namespace Application.Services.Query
{
    public class BookingCheckOutQuery : IBookingCheckOutQuery
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IReadAllResourcesQuery _resourcesQuery;
        private readonly ResourceFilterDto _filter = new ResourceFilterDto { IsAvailable = false };

        public BookingCheckOutQuery(IBookingRepository bookingRepository, IReadAllResourcesQuery resourcesQuery)
        {
            _bookingRepository = bookingRepository;
            _resourcesQuery = resourcesQuery;
        }

        public async Task<IResult<List<ReadBookingMissingCheckOutQueryResponseDto>>> GetFinishedBookingsWithMissingCheckOutsAsync()
        {
            // Creates tasks to run
            Task<IResult<IEnumerable<Booking>>> missingCheckOutTask = _bookingRepository.GetFinishedBookingsWithMissingCheckOutsAsync();
            Task<IResult<IEnumerable<ReadResourceQueryResponseDto>>> resourcesTask = _resourcesQuery.ReadAllResourcesAsync(_filter);

            // Runs both task simultaneously, so they don't wait for each other
            await Task.WhenAll(missingCheckOutTask, resourcesTask);

            // Get task results
            IResult<IEnumerable<Booking>> missingCheckOutTaskResult = missingCheckOutTask.Result;
            IResult<IEnumerable<ReadResourceQueryResponseDto>> resourcesTaskResult = resourcesTask.Result;

            // Failed to get bookings return error message
            if (missingCheckOutTaskResult.IsSucces() == false)
            {
                return Result<List<ReadBookingMissingCheckOutQueryResponseDto>>.Error(null!, new Exception("Kunne ikke hente bookinger fra databasen."));
            }

            // Failed to get resources return error message
            if (resourcesTaskResult.IsSucces() == false)
            {
                return Result<List<ReadBookingMissingCheckOutQueryResponseDto>>.Error(null!, new Exception("Kunne ikke hente ressourcer fra databasen."));
            }

            // Get success results
            IEnumerable<Booking> missingCheckOut = missingCheckOutTaskResult.GetSuccess().OriginalType;
            IEnumerable<ReadResourceQueryResponseDto> resources = resourcesTaskResult.GetSuccess().OriginalType;

            // Mapping the resources to a dictionary for instant look up
            Dictionary<int, ReadResourceQueryResponseDto> resourceMap = resources.ToDictionary(resource => resource.Id, resource => resource);

            // Creates the response list to return
            List<ReadBookingMissingCheckOutQueryResponseDto> responseList = new List<ReadBookingMissingCheckOutQueryResponseDto>();

            foreach (Booking booking in missingCheckOut) // Iterates through each booking and "converts" them into a dto.
            {
                // Gets value from dictionary
                if (resourceMap.TryGetValue(booking.ResourceId, out ReadResourceQueryResponseDto? matchingResource))
                {
                    ReadBookingMissingCheckOutQueryResponseDto missingCheckoutInfo = new ReadBookingMissingCheckOutQueryResponseDto
                    {
                        BookingId = booking.Id,
                        ResourceName = matchingResource.Name,
                        ResourceLocation = matchingResource.Location,
                        BookingStartDate = booking.StartDate,
                        BookingEndDate = booking.StartDate,
                        GuestName = $"{booking.Guest.FirstName} {booking.Guest.LastName}"
                    };

                    responseList.Add(missingCheckoutInfo);
                }
            }

            if (responseList.Any())
            {
                return Result<List<ReadBookingMissingCheckOutQueryResponseDto>>.Success(responseList);
            }

            return Result<List<ReadBookingMissingCheckOutQueryResponseDto>>.Error(responseList, new Exception("Der er ingen manglende indtjekninger."));
        }
    }
}