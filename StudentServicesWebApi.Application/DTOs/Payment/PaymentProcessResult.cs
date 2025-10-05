namespace StudentServicesWebApi.Application.DTOs.Payment;

/// <summary>
/// Result of payment processing operation
/// </summary>
public class PaymentProcessResult
{
    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Result message (e.g., "AlreadySuccess", "ProcessedSuccessfully", etc.)
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The processed payment details (null if operation failed or payment already processed)
    /// </summary>
    public PaymentDto? Payment { get; set; }

    /// <summary>
    /// Result code for specific handling
    /// </summary>
    public PaymentProcessResultCode ResultCode { get; set; }
}

/// <summary>
/// Specific result codes for payment processing
/// </summary>
public enum PaymentProcessResultCode
{
    Success = 1,
    AlreadySuccess = 2,
    InvalidTransition = 3,
    ValidationError = 4,
    Unauthorized = 5,
    NotFound = 6
}