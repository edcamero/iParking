namespace iParking.Domain.Shared
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public static ApiResponse<T> Success(T data, string message = null) =>
            new ApiResponse<T> { Status = true, Data = data, Message = message };

        public static ApiResponse<T> Failure(string message, T data = default) =>
            new ApiResponse<T> { Status = false, Data = data, Message = message };
    }
}
