namespace LikeBusLogistic.BLL.Results
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class BaseResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
