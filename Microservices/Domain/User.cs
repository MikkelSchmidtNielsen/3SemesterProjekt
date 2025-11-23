namespace Domain
{
    public sealed class User
    {
        public int Id { get; init; }
        public string Email { get; private set; }

        public User(string email)
        {
            Email = email;
        }
    }
}
