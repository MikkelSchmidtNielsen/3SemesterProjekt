using MimeKit;

namespace Domain
{
    public sealed class User
    {
        public int Id { get; init; }
        public string Email { get; private set; }

        private User() { }

        public User(string email)
        {
            Email = email;

            ValidateInformation();
        }

        private void ValidateInformation()
        {
            // Validates the email based on RFC-standards
            if (MailboxAddress.TryParse(Email, out MailboxAddress mail) == false)
            {
                throw new Exception("Email was in wrong format");
            }

            if (mail.Address.Contains(".") == false)
            {
                throw new Exception("Email was in wrong format. Does not include an end address");
            }
        }
    }
}
