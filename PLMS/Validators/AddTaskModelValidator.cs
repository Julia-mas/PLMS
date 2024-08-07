using FluentValidation;
using PLMS.API.Models.ModelsTasks;

namespace PLMS.API.Validators
{
    public class AddTaskModelValidator : AbstractValidator<AddTaskModel>
    {
        public AddTaskModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description can't exceed 500 characters.");
            RuleFor(x => x.PriorityId).Must(id => new List<int> { 1, 2, 3 }.Contains(id))
            .WithMessage("Priority Id must be 1, 2, or 3.");
            RuleFor(x => x.StatusId).Must(id => new List<int> { 1, 2, 3, 4, 5, 6 }.Contains(id))
            .WithMessage("Status Id must be 1, 2, 3, 4, 5 or 6.");
            RuleFor(x => x.Goal.Title).MaximumLength(200).WithMessage("Description can't exceed 200 characters.");
        }
    }
}
