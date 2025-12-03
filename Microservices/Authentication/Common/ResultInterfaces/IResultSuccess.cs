namespace Common.ResultInterfaces
{
    public interface IResultSuccess<T>
    {
        T OriginalType { get; }
    }
}
