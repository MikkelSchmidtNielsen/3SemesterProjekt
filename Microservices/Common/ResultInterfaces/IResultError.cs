namespace Common.ResultInterfaces
{
    public interface IResultError<T> : IResultSuccess<T>
    {
        Exception? Exception { get; }
    }
}
