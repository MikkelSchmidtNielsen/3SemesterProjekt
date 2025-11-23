namespace Common.ResultInterfaces
{
    public interface IResultConflict<T> : IResultError<T>, IResultSuccess<T>
    {
        T? CurrentType { get; }
    }
}
