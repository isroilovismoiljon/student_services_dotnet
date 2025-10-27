using FluentValidation;
using StudentServicesWebApi.Application.DTOs.Presentation;

namespace StudentServicesWebApi.Application.Validators;

public class CreatePresentationMixedDtoValidator : AbstractValidator<CreatePresentationMixedDto>
{
    public CreatePresentationMixedDtoValidator()
    {
        RuleFor(x => x.TitleText)
            .NotEmpty()
            .WithMessage("Title text is required");

        RuleFor(x => x.TitleFont)
            .NotEmpty()
            .WithMessage("Title font is required");

        RuleFor(x => x.TitleSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Title font size must be between 1 and 100");

        RuleFor(x => x.TitleLeft)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Title Left position must be between 0 and 33.867 cm");

        RuleFor(x => x.TitleTop)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Title Top position must be between 0 and 19.05 cm");

        RuleFor(x => x.TitleWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Title Width must be between 1 and 33.867 cm");

        RuleFor(x => x.TitleHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Title Height must be between 1 and 19.05 cm");

        // Author validation rules
        RuleFor(x => x.AuthorText)
            .NotEmpty()
            .WithMessage("Author text is required");

        RuleFor(x => x.AuthorFont)
            .NotEmpty()
            .WithMessage("Author font is required");

        RuleFor(x => x.AuthorSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Author font size must be between 1 and 100");

        RuleFor(x => x.AuthorLeft)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Author Left position must be between 0 and 33.867 cm");

        RuleFor(x => x.AuthorTop)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Author Top position must be between 0 and 19.05 cm");

        RuleFor(x => x.AuthorWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Author Width must be between 1 and 33.867 cm");

        RuleFor(x => x.AuthorHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Author Height must be between 1 and 19.05 cm");

        // Plan validation rules
        RuleFor(x => x.PlanText)
            .NotEmpty()
            .WithMessage("Plan text is required");

        RuleFor(x => x.PlanFont)
            .NotEmpty()
            .WithMessage("Plan font is required");

        RuleFor(x => x.PlanSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Plan font size must be between 1 and 100");

        RuleFor(x => x.PlanLeft)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Plan Left position must be between 0 and 33.867 cm");

        RuleFor(x => x.PlanTop)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Plan Top position must be between 0 and 19.05 cm");

        RuleFor(x => x.PlanWidth)
            .GreaterThan(0)
            .LessThanOrEqualTo(33.867)
            .WithMessage("Plan Width must be between 1 and 33.867 cm");

        RuleFor(x => x.PlanHeight)
            .GreaterThan(0)
            .LessThanOrEqualTo(19.05)
            .WithMessage("Plan Height must be between 1 and 19.05 cm");

        // General validation rules
        RuleFor(x => x.DesignId)
            .GreaterThan(0)
            .WithMessage("Design ID must be greater than 0");

        RuleFor(x => x.PageCount)
            .InclusiveBetween(1, 100)
            .WithMessage("Page count must be between 1 and 100");

        // Conditional validation for photos when WithPhoto is true
        RuleFor(x => x.Photos)
            .Must((dto, photos) => 
            {
                if (!dto.WithPhoto) return true;
                var requiredCount = (dto.PageCount - 2) / 2;
                return photos != null && photos.Count == requiredCount;
            })
            .WithMessage(dto => 
            {
                if (!dto.WithPhoto) return "";
                var requiredCount = (dto.PageCount - 2) / 2;
                return $"When WithPhoto is true and PageCount is {dto.PageCount}, exactly {requiredCount} photos are required";
            });

        // Validate that no photos are provided when WithPhoto is false
        RuleFor(x => x.Photos)
            .Must((dto, photos) => 
            {
                if (dto.WithPhoto) return true;
                return photos == null || photos.Count == 0;
            })
            .WithMessage("No photos should be provided when WithPhoto is false");

        // PostTexts JSON validation - always required
        RuleFor(x => x.PostTexts)
            .NotEmpty()
            .WithMessage("PostTexts is required")
            .Must(BeValidCreateTextSlideDtoArrayJson)
            .WithMessage("PostTexts must be a valid JSON array of CreateTextSlideDto objects");
            
        // PhotoPositions JSON validation when WithPhoto is true
        When(x => x.WithPhoto, () => {
            RuleFor(x => x.PhotoPositions)
                .NotEmpty()
                .WithMessage("PhotoPositions is required when WithPhoto is true")
                .Must(BeValidPhotoPositionArrayJson)
                .WithMessage("PhotoPositions must be a valid JSON array of PhotoPositionDto objects");
        });
        
        // Ensure PhotoPositions is empty when WithPhoto is false
        When(x => !x.WithPhoto, () => {
            RuleFor(x => x.PhotoPositions)
                .Must(positions => string.IsNullOrEmpty(positions))
                .WithMessage("PhotoPositions should be empty when WithPhoto is false");
        });
    }

    private bool BeValidCreateTextSlideDtoArrayJson(string postTexts)
    {
        if (string.IsNullOrWhiteSpace(postTexts)) return false;
        
        try
        {
            var slides = System.Text.Json.JsonSerializer.Deserialize<List<StudentServicesWebApi.Application.DTOs.TextSlide.CreateTextSlideDto>>(postTexts, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            // Validate that it's a non-empty array
            return slides != null && slides.Count > 0;
        }
        catch
        {
            return false;
        }
    }
    
    private bool BeValidPhotoPositionArrayJson(string photoPositions)
    {
        if (string.IsNullOrWhiteSpace(photoPositions)) return false;
        
        try
        {
            var positions = System.Text.Json.JsonSerializer.Deserialize<List<StudentServicesWebApi.Application.DTOs.Presentation.PhotoPositionDto>>(photoPositions, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            // Validate that it's a valid array
            if (positions == null) return false;
            
            // Validate each position has valid ranges
            foreach (var pos in positions)
            {
                if (pos.Left < 0 || pos.Left > 33.867) return false;
                if (pos.Top < 0 || pos.Top > 19.05) return false;
                if (pos.Width <= 0 || pos.Width > 33.867) return false;
                if (pos.Height.HasValue && (pos.Height <= 0 || pos.Height > 19.05)) return false;
                
                // Check slide boundaries
                if (pos.Left + pos.Width > 33.867) return false;
                if (pos.Height.HasValue && pos.Top + pos.Height > 19.05) return false;
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }
}
