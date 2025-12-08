namespace Domain.Models
{
    public sealed class User
    {
        public int Id { get; init; }
        public string Email { get; private set; }
        public int Otp { get; private set; }
        public DateTime OtpExpiryTime { get; private set; }

        public User(string email)
        {
            Email = email;

            ValidateInformation();
        }


        private void ValidateInformation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentException("Email skal være angivet", nameof(Email));
            }

            int atIndex = Email.IndexOf('@');
            int dotIndex = Email.LastIndexOf('.');

            if (atIndex <= 0 || dotIndex <= atIndex + 1 || dotIndex == Email.Length - 1)
            {
                throw new ArgumentException("Fejl i @ og/eller .");
            }
        }

        public void SetOtp(int otp, DateTime expiryTime)
        {
            Otp = otp;
            OtpExpiryTime = expiryTime;
        }
    }
}
