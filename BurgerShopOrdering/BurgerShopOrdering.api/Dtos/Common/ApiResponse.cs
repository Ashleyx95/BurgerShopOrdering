namespace BurgerShopOrdering.api.Dtos.Common
{
    public class ApiResponse<T>(bool success, string message, IEnumerable<string>? errors = null, T? data = default) where T : class
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
        public IEnumerable<string> Errors { get; set; } = errors ?? Array.Empty<string>();
        public T? Data { get; set; } = data;

        public static ApiResponse<T> SuccessResponse(T? data, string message = "")
            => new ApiResponse<T>(true, message, data: data);

        public static ApiResponse<T> FailureResponse(string message, IEnumerable<string>? errors = null)
            => new ApiResponse<T>(false, message, errors);
    }
}
