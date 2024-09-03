using FluentValidation;
using PLMS.API.Models.ModelsTaskComments;
using PLMS.Common;

namespace PLMS.API.Validators
{
    public partial class CommentModelValidator
    {
        public partial class TaskCommentModelValidator : AbstractValidator<TaskCommentModel>
        {
            public TaskCommentModelValidator()
            {
                RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.");
                RuleFor(x => x.Comment).MaximumLength(Constants.Comment.CommentMaxLength)
                    .WithMessage($"Comment can't exceed [{Constants.Comment.CommentMaxLength}] characters.");
            }
        }
    }  
}

