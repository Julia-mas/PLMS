using FluentValidation;
using PLMS.API.Models.ModelsCategories;
using PLMS.Common;

namespace PLMS.API.Validators
{
    public class CategoryBaseModelValidator : AbstractValidator<CategoryBaseModel>
    {
        public CategoryBaseModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Title).MaximumLength(Constants.Category.TitleMaxLength)
                .WithMessage($"Title can't exceed [{Constants.Category.TitleMaxLength}] characters.");
        }
    }
}

