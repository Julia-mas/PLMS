using FluentValidation;
using PLMS.API.Models.ModelsGoalComments;
using PLMS.API.Models.ModelsTaskComments;
using PLMS.Common;

namespace PLMS.API.Validators
{
    public class CommentModelValidator
    {
        public partial class GoalCommentModelValidator : AbstractValidator<GoalCommentModel>
        {
            public GoalCommentModelValidator()
            {
                RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.");
                RuleFor(x => x.Comment).MaximumLength(Constants.Comment.CommentMaxLength)
                    .WithMessage($"Title can't exceed [{Constants.Comment.CommentMaxLength}] characters.");
            }
        }

        public partial class TaskCommentModelValidator : AbstractValidator<TaskCommentModel>
        {
            public TaskCommentModelValidator()
            {
                RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.");
                RuleFor(x => x.Comment).MaximumLength(Constants.Comment.CommentMaxLength)
                    .WithMessage($"Title can't exceed [{Constants.Comment.CommentMaxLength}] characters.");
            }
        }
    }  
}

