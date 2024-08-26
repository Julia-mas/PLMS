using AutoMapper;
using PLMS.BLL.DTO.GoalCommentsDto;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using PLMS.DAL.Entities;
using PLMS.DAL.Interfaces;
using Task = System.Threading.Tasks.Task;


namespace PLMS.BLL.ServicesImplementation
{
    public class GoalCommentService : IGoalCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IRepository<GoalComment> _goalCommentRepository;

        public GoalCommentService(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
            _goalCommentRepository = _unitOfWork.GetRepository<GoalComment>();
        }

        public async Task<int> AddGoalComment(AddGoalCommentDto commentDto, string userId)
        {
            if (!await _permissionService.HasPermissionToGoal(commentDto.GoalId, userId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            var taskComment = _mapper.Map<GoalComment>(commentDto);
            await _goalCommentRepository.CreateAsync(taskComment);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return taskComment.Id;
        }

        public async Task EditGoalComment(EditGoalCommentDto commentDto, string userId)
        {
            var goalComment = await _goalCommentRepository.GetByPredicateAsync(gc => gc.Id == commentDto.Id) ?? throw new NotFoundException("Goal comment was not found");
            if (!await _permissionService.HasPermissionToGoal(goalComment.GoalId, userId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            goalComment.Comment = commentDto.Comment != default ? commentDto.Comment : goalComment.Comment;

            _goalCommentRepository.Update(goalComment);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task DeleteGoalComment(int id, string userId)
        {
            GoalComment? goalComment = await _goalCommentRepository.GetByPredicateAsync(gc => gc.Id.Equals(id)) ?? throw new NotFoundException("Goal comment was not found");
            if (goalComment == null)
            {
                return;
            }
            if (!await _permissionService.HasPermissionToGoal(goalComment.GoalId, userId))
            {
                throw new NotFoundException("Goal was not found.");
            }

            _goalCommentRepository.Remove(goalComment);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<GetGoalCommentDto> GetGoalCommentByIdAsync(int id, string userId)
        {
            var goalComment = await _goalCommentRepository.GetByPredicateAsync(gc => gc.Id.Equals(id)) ?? throw new NotFoundException("Goal comment was not found");

            if (!await _permissionService.HasPermissionToGoal(goalComment.GoalId, userId))
            {
                throw new NotFoundException("Goal comment was not found.");
            }

            var goalCommentDto = _mapper.Map<GetGoalCommentDto>(goalComment);

            return goalCommentDto;
        }
    }
}
