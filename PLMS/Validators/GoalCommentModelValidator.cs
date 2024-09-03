using FluentValidation;
using PLMS.API.Models.ModelsGoalComments;
using PLMS.Common;

namespace PLMS.API.Validators
{   
    public partial class GoalCommentModelValidator : AbstractValidator<GoalCommentModel>
    {
        public GoalCommentModelValidator()
        {
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.");
            RuleFor(x => x.Comment).MaximumLength(Constants.Comment.CommentMaxLength)
                .WithMessage($"Comment can't exceed [{Constants.Comment.CommentMaxLength}] characters.");
        }
    }
}

