namespace TreasureGuide.Web.Services.API.Generic
{
    public interface IDataService<TKey>
        where TKey : struct
    {
        DataResult<TOutput> Get<TOutput>(TKey? id = null);
        DataResult<TKey> Post<TInput>(TInput model, TKey? id = null);
        DataResult<bool> Delete(TKey? id = null);
    }

    public struct DataResult<TType>
    {
        public ErrorType ErrorType { get; set; }
        public object ErrorData { get; set; }
        public TType Result { get; set; }
    }

    public enum ErrorType
    {
        None = 0,
        NotFound,
        BadRequest,
        Forbidden,
        NoContent
    }
}
