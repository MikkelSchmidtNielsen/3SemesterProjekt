using Application.ApplicationDto.Query;
using Domain.Models;

namespace Application.InfrastructureInterfaces.SendEmailSpecifications
{
	public class SendOrderConfirmationEmail : ISendEmailSpecification
	{

		public string RecieverEmail { get; }

		public string Subject { get; }

		public string Body { get; }

		private static readonly TimeOnly DefaultCheckIn = new(12, 0);
		private static readonly TimeOnly DefaultCheckOut = new(11, 0);

		public SendOrderConfirmationEmail(Booking booking, Guest guest, ReadResourceByIdQueryResponseDto resource)
		{
			RecieverEmail = guest.Email!;

			Subject = "Order Confirmation";

			Body = GenerateBody(booking, guest, resource);
		}

		protected static string GenerateBody(Booking booking, Guest guest, ReadResourceByIdQueryResponseDto resource)
		{
			if (guest.FirstName is null || string.IsNullOrWhiteSpace(guest.FirstName))
			{
				throw new Exception("Missing Information About the booking");
			}

			return $@"
					Hej {guest.FirstName},

					Tak for din booking!

					Her er detaljerne for din reservation:

					Ressource:
					- Navn: {resource.Name}
					- Type: {resource.Type}

					Periode:
					- Startdato: {booking.StartDate:dd-MM-yyyy}
					- Slutdato : {booking.EndDate:dd-MM-yyyy}

					Check-ind og check-ud:
					- Check-ind: {DefaultCheckIn:HH\\:mm}
					- Check-ud : {DefaultCheckOut:HH\\:mm}

					Pris:
					- Totalpris: {booking.TotalPrice:C}

					Vi glæder os til at byde dig velkommen!

					Venlig hilsen
					Dit Bookingteam
					";
		}

	}
}
