namespace UltimateERP.Application.Common.Models;

/// <summary>
/// Standardized API response wrapper matching Dynamic ERP convention:
/// { Data, TotalCount, IsSuccess, ResponseMSG }
/// </summary>
public class ApiResponse<T>
{
    public T? Data { get; set; }
    public int TotalCount { get; set; }
    public bool IsSuccess { get; set; }
    public string ResponseMSG { get; set; } = string.Empty;

    public static ApiResponse<T> Success(T data, string message = "Success", int totalCount = 0)
    {
        return new ApiResponse<T>
        {
            Data = data,
            TotalCount = totalCount,
            IsSuccess = true,
            ResponseMSG = message
        };
    }

    public static ApiResponse<T> Failure(string message)
    {
        return new ApiResponse<T>
        {
            Data = default,
            TotalCount = 0,
            IsSuccess = false,
            ResponseMSG = message
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Success(string message = "Success")
    {
        return new ApiResponse
        {
            Data = null,
            TotalCount = 0,
            IsSuccess = true,
            ResponseMSG = message
        };
    }

    public new static ApiResponse Failure(string message)
    {
        return new ApiResponse
        {
            Data = null,
            TotalCount = 0,
            IsSuccess = false,
            ResponseMSG = message
        };
    }
}
