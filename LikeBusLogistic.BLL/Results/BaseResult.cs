namespace LikeBusLogistic.BLL.Results
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OriginalString { get; set; }
    }
    public class BaseResult<T> : BaseResult
    {
        public T Data { get; set; }
    }
}
