using AutoMapper;
using PLMS.BLL.DTO.TaskCommentsDto;
using PLMS.BLL.ServicesInterfaces;
using PLMS.Common.Exceptions;
using PLMS.DAL.Entities;
using PLMS.DAL.Interfaces;
using Task = System.Threading.Tasks.Task;


namespace PLMS.BLL.ServicesImplementation
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<TaskComment> _taskCommentRepository;
        private readonly IPermissionService _permissionService;

        public TaskCommentService(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
            _taskCommentRepository = _unitOfWork.GetRepository<TaskComment>();
        }

        public async Task<int> AddTaskComment(AddTaskCommentDto commentDto, string userId)
        {
            if (!await _permissionService.HasPermissionToGoalAndTask(commentDto.TaskId, userId))
            {
                throw new NotFoundException("Task was not found.");
            }

            var taskComment = _mapper.Map<TaskComment>(commentDto);
            await _taskCommentRepository.CreateAsync(taskComment);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return taskComment.Id;
        }

        public async Task EditTaskComment(EditTaskCommentDto commentDto, string userId)
        {
            var taskComment = await _taskCommentRepository.GetByPredicateAsync(tc => tc.Id == commentDto.Id) ?? throw new NotFoundException("GoalComment was not found");
            if (!await _permissionService.HasPermissionToGoalAndTask(taskComment.TaskId, userId))
            {
                throw new NotFoundException("Task was not found.");
            }

            taskComment.Comment = commentDto.Comment != default ? commentDto.Comment : taskComment.Comment;

            _taskCommentRepository.Update(taskComment);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task DeleteTaskComment(int id, string userId)
        {
            TaskComment? taskComment = await _taskCommentRepository.GetByPredicateAsync(tc => tc.Id.Equals(id)) ?? throw new NotFoundException("TaskComment was not found");
            if (taskComment == null)
            {
                return;
            }
            if (!await _permissionService.HasPermissionToGoalAndTask(taskComment.TaskId, userId))
            {
                throw new NotFoundException("Task was not found.");
            }

            _taskCommentRepository.Remove(taskComment);
            await _unitOfWork.CommitChangesToDatabaseAsync();
        }

        public async Task<GetTaskCommentDto> GetTaskCommentByIdAsync(int id, string userId)
        {
            var taskComment = await _taskCommentRepository.GetByPredicateAsync(gc => gc.Id.Equals(id)) ?? throw new NotFoundException("Task comment was not found");

            if (!await _permissionService.HasPermissionToGoal(taskComment.TaskId, userId))
            {
                throw new NotFoundException("Task comment was not found.");
            }

            var goalCommentDto = _mapper.Map<GetTaskCommentDto>(taskComment);

            return goalCommentDto;
        }
    }
}
