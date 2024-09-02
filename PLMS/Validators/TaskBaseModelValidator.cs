using FluentValidation;
using PLMS.API.Models.ModelsTasks;
using PLMS.Common;
using PLMS.Common.Enums;
using PLMS.Common.Extensions;

namespace PLMS.API.Validators
{
    public partial class TaskBaseModelValidator : AbstractValidator<TaskBaseModel>
    {
        public TaskBaseModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Title).MaximumLength(Constants.Task.TitleMaxLength)
                .WithMessage($"Comment can't exceed [{Constants.Task.TitleMaxLength}] characters.");

            RuleFor(x => x.Description).MaximumLength(Constants.Task.DescriptionMaxLength)
                .WithMessage($"Description can't exceed [{Constants.Task.DescriptionMaxLength}] characters.");

            var priorityIds = EnumExtensions.GetEnumValues<PriorityEnum>().ToArray();
            var statusIds = EnumExtensions.GetEnumValues<StatusEnum>().ToArray();

            RuleFor(x => x.PriorityId).Must(id => priorityIds.Contains(id))
                .WithMessage($"Priority Id must present in [{string.Join(", ", priorityIds)}] values");

            RuleFor(x => x.StatusId).Must(id => statusIds.Contains(id))
                .WithMessage($"Status Id must present in [{string.Join(", ", statusIds)}] values ");
        }
    }
}
