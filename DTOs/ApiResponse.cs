
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Timestamp = DateTime.UtcNow
        };
    }

    public static ApiResponse<T> SuccessMessage(string message = "Operation completed successfully.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = default,
            Timestamp = DateTime.UtcNow
        };
    }

    public static ApiResponse<T> Fail(string message = "The operation failed.", List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };
    }

    public static ApiResponse<T> NotFound(string message = "The requested resource was not found.")
    {
        return Fail(message);
    }

    public static ApiResponse<T> Unauthorized(string message = "You are not authorized to perform this action.")
    {
        return Fail(message);
    }


    public static ApiResponse<T> ValidationError(List<string> errors, string message = "Validation failed.")
    {
        return Fail(message, errors);
    }

    public static ApiResponse<T> ServerError(string message = "An unexpected error occurred.")
    {
        return Fail(message);
    }
}
