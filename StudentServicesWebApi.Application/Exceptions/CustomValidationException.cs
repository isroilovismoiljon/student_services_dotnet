namespace StudentServicesWebApi.Application.Exceptions;
public class CustomValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }
    public string? LocalizationKey { get; }
    public CustomValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>
        {
            { "General", new[] { message } }
        };
        LocalizationKey = null;
    }
    public CustomValidationException(string localizationKey, string fieldName = "General") : base(localizationKey)
    {
        LocalizationKey = localizationKey;
        Errors = new Dictionary<string, string[]>
        {
            { fieldName, new[] { localizationKey } }
        };
    }
    public CustomValidationException(IDictionary<string, string[]> errors)
        : base(GetErrorMessage(errors))
    {
        Errors = errors;
        LocalizationKey = null;
    }
    private static string GetErrorMessage(IDictionary<string, string[]> errors)
    {
        if (errors == null || !errors.Any())
            return "Validation failed";
        var firstError = errors.First();
        return firstError.Value?.FirstOrDefault() ?? "Validation failed";
    }
    public CustomValidationException(IEnumerable<(string Field, string Error)> errors)
        : base(GetErrorMessage(errors))
    {
        Errors = errors
            .GroupBy(e => e.Field)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Error).ToArray()
            );
        LocalizationKey = null;
    }
    private static string GetErrorMessage(IEnumerable<(string Field, string Error)> errors)
    {
        return errors?.FirstOrDefault().Error ?? "Validation failed";
    }
}
