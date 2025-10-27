using FluentValidation;
using StudentServicesWebApi.Application.DTOs.OpenaiKey;

namespace StudentServicesWebApi.Application.Validators.OpenaiKey;

public class CreateOpenaiKeyDtoValidator : AbstractValidator<CreateOpenaiKeyDto>
{
    public CreateOpenaiKeyDtoValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("OpenAI API Key is required")
            .Length(20, 256)
            .WithMessage("Key must be between 20 and 256 characters")
            .Matches(@"^sk-[a-zA-Z0-9]{20,}$")
            .WithMessage("Key must start with 'sk-' followed by alphanumeric characters");
    }
}

public class UpdateOpenaiKeyDtoValidator : AbstractValidator<UpdateOpenaiKeyDto>
{
    public UpdateOpenaiKeyDtoValidator()
    {
        RuleFor(x => x.Key)
            .Length(20, 256)
            .WithMessage("Key must be between 20 and 256 characters")
            .Matches(@"^sk-[a-zA-Z0-9]{20,}$")
            .WithMessage("Key must start with 'sk-' followed by alphanumeric characters")
            .When(x => !string.IsNullOrEmpty(x.Key));
    }
}

public class BulkCreateOpenaiKeyDtoValidator : AbstractValidator<BulkCreateOpenaiKeyDto>
{
    public BulkCreateOpenaiKeyDtoValidator()
    {
        RuleFor(x => x.OpenaiKeys)
            .NotNull()
            .WithMessage("OpenaiKeys list is required")
            .NotEmpty()
            .WithMessage("At least one OpenAI key is required")
            .Must(keys => keys.Count <= 100)
            .WithMessage("Cannot create more than 100 OpenAI keys at once");

        RuleForEach(x => x.OpenaiKeys)
            .SetValidator(new CreateOpenaiKeyDtoValidator());

        RuleFor(x => x.OpenaiKeys)
            .Must(BeUniqueKeys)
            .WithMessage("Duplicate keys are not allowed in the same request");
    }

    private static bool BeUniqueKeys(List<CreateOpenaiKeyDto> keys)
    {
        if (keys == null || keys.Count <= 1) return true;
        
        var keyValues = keys.Select(k => k.Key).ToList();
        return keyValues.Count == keyValues.Distinct().Count();
    }
}

public class BulkOpenaiKeyOperationDtoValidator : AbstractValidator<BulkOpenaiKeyOperationDto>
{
    public BulkOpenaiKeyOperationDtoValidator()
    {
        RuleFor(x => x.OpenaiKeyIds)
            .NotNull()
            .WithMessage("OpenaiKeyIds list is required")
            .NotEmpty()
            .WithMessage("At least one OpenAI key ID is required")
            .Must(ids => ids.Count <= 100)
            .WithMessage("Cannot operate on more than 100 OpenAI keys at once");

        RuleForEach(x => x.OpenaiKeyIds)
            .GreaterThan(0)
            .WithMessage("OpenAI key ID must be a positive integer");

        RuleFor(x => x.OpenaiKeyIds)
            .Must(BeUniqueIds)
            .WithMessage("Duplicate IDs are not allowed in the same request");
    }

    private static bool BeUniqueIds(List<int> ids)
    {
        if (ids == null || ids.Count <= 1) return true;
        
        return ids.Count == ids.Distinct().Count();
    }
}

public class IncrementUsageDtoValidator : AbstractValidator<IncrementUsageDto>
{
    public IncrementUsageDtoValidator()
    {
        RuleFor(x => x.IncrementBy)
            .InclusiveBetween(1, 1000)
            .WithMessage("Increment amount must be between 1 and 1000");
    }
}