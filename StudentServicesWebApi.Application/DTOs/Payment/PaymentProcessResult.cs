namespace StudentServicesWebApi.Application.DTOs.Payment;
public class PaymentProcessResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public PaymentDto? Payment { get; set; }
    public PaymentProcessResultCode ResultCode { get; set; }
}
public enum PaymentProcessResultCode
{
    Success = 1,
    AlreadySuccess = 2,
    InvalidTransition = 3,
    ValidationError = 4,
    Unauthorized = 5,
    NotFound = 6
}
