namespace Common.ResultInterfaces
{
    public interface IResult<T>
    {
        bool IsSuccess();
        IResultSuccess<T> GetSuccess();
        bool IsError();
        IResultError<T> GetError();
        bool IsConflict();
        IResultConflict<T> GetConflict();
    }
}
