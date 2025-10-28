using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentServicesWebApi.Domain.Enums;
using StudentServicesWebApi.Domain.Interfaces;
using StudentServicesWebApi.Infrastructure.Interfaces;
namespace StudentServicesWebApi.Attributes;
public class ValidateUserCanCreatePaymentAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        {
            context.Result = new UnauthorizedObjectResult("Invalid user token.");
            return;
        }
        var user = await userService.GetUserByIdAsync(userId);
        if (user == null || user.UserRole != UserRole.User)
        {
            context.Result = new ForbidResult("Only users with User role can create payments.");
            return;
        }
        await next();
    }
}
public class ValidateAdminCanProcessPaymentAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        {
            context.Result = new UnauthorizedObjectResult("Invalid user token.");
            return;
        }
        var user = await userService.GetUserByIdAsync(userId);
        if (user == null || (user.UserRole != UserRole.Admin && user.UserRole != UserRole.SuperAdmin))
        {
            context.Result = new ForbidResult("Only Admins and SuperAdmins can process payments.");
            return;
        }
        await next();
    }
}
public class ValidatePaymentAccessAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var paymentService = context.HttpContext.RequestServices.GetRequiredService<IPaymentService>();
        if (!context.RouteData.Values.TryGetValue("id", out var paymentIdObj) ||
            !int.TryParse(paymentIdObj?.ToString(), out int paymentId))
        {
            context.Result = new BadRequestObjectResult("Invalid payment ID.");
            return;
        }
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        {
            context.Result = new UnauthorizedObjectResult("Invalid user token.");
            return;
        }
        var payment = await paymentService.GetPaymentByIdAsync(paymentId);
        if (payment == null)
        {
            context.Result = new NotFoundObjectResult("Payment not found.");
            return;
        }
        var userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole == "SuperAdmin")
        {
            await next();
            return;
        }
        if (payment.Sender.Id != userId && userRole != "Admin")
        {
            context.Result = new ForbidResult("You are not authorized to access this payment.");
            return;
        }
        await next();
    }
}
public class ValidatePaymentReceiptFileAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var fileUploadService = context.HttpContext.RequestServices.GetRequiredService<IFileUploadService>();
        var fileParameter = context.ActionArguments.Values
            .OfType<Microsoft.AspNetCore.Http.IFormFile>()
            .FirstOrDefault();
        if (fileParameter == null)
        {
            var createPaymentDto = context.ActionArguments.Values
                .FirstOrDefault(arg => arg?.GetType().Name == "CreatePaymentDto");
            if (createPaymentDto != null)
            {
                var photoProperty = createPaymentDto.GetType().GetProperty("Photo");
                fileParameter = photoProperty?.GetValue(createPaymentDto) as Microsoft.AspNetCore.Http.IFormFile;
            }
        }
        if (fileParameter == null)
        {
            context.Result = new BadRequestObjectResult("Payment receipt file is required.");
            return;
        }
        if (!fileUploadService.IsValidPaymentReceiptFile(fileParameter))
        {
            var allowedExtensions = string.Join(", ", fileUploadService.GetAllowedExtensions());
            var maxSizeMB = fileUploadService.GetMaxFileSize() / (1024 * 1024);
            context.Result = new BadRequestObjectResult(
                $"Invalid file. Allowed formats: {allowedExtensions}. Maximum size: {maxSizeMB}MB.");
            return;
        }
        await next();
    }
}
public class PaymentRateLimitAttribute : ActionFilterAttribute
{
    private readonly int _maxRequestsPerMinute;
    private readonly string _keyPrefix;
    private static readonly Dictionary<string, List<DateTime>> _requestLog = new();
    private static readonly object _lock = new object();
    public PaymentRateLimitAttribute(int maxRequestsPerMinute = 5, string keyPrefix = "payment")
    {
        _maxRequestsPerMinute = maxRequestsPerMinute;
        _keyPrefix = keyPrefix;
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            await next();
            return;
        }
        var key = $"{_keyPrefix}_{userIdClaim}";
        var now = DateTime.UtcNow;
        var windowStart = now.AddMinutes(-1);
        lock (_lock)
        {
            if (!_requestLog.ContainsKey(key))
            {
                _requestLog[key] = new List<DateTime>();
            }
            var userRequests = _requestLog[key];
            userRequests.RemoveAll(time => time < windowStart);
            if (userRequests.Count >= _maxRequestsPerMinute)
            {
                context.Result = new ObjectResult("Too many requests. Please try again later.")
                {
                    StatusCode = 429
                };
                return;
            }
            userRequests.Add(now);
        }
        await next();
    }
}
