namespace LikeBusLogistic.Web.Models
{
    public class Result
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
