namespace Common.Exceptions
{
	/// <summary>
	/// Used as a Middleware exception. Should be thrown when wanting to return a Status409Conflict
	/// </summary>
	public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
