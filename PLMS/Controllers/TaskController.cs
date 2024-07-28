using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLMS.DAL.Filters;
using PLMS.DAL.Interfaces;
using Task = PMLS.DAL.Entities.Task;

namespace PLMS.API.Controllers
{
    [Route("plms/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private IRepository<Task> _taskRepository;

        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _taskRepository = _unitOfWork.GetRepository<PMLS.DAL.Entities.Task>();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PMLS.DAL.Entities.Task>> GetById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, Task task)
        {
            if (task.Id != id)
            {
                return BadRequest();
            }

            _taskRepository.Update(task);

            try
            {
                await _unitOfWork.CommitChangesToDatabaseAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existingTask = await _taskRepository.GetByIdAsync(id);
                if (existingTask == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Task>> Add(Task task)
        {
            _taskRepository.Create(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            Task task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _taskRepository.Remove(task);
            await _unitOfWork.CommitChangesToDatabaseAsync();

            return NoContent();
        }

        [HttpGet("GetFilteredShort")]
        public async Task<ActionResult<IEnumerable<Task>>> GetFilteredShort([FromQuery] MyItemFilter filters,
            [FromQuery] string sortField, [FromQuery] string includeColumns)
        {
            Func<IQueryable<Task>, IOrderedQueryable<Task>>? orderBy = null;
            if (sortField == "CreatedAt")
            {
                orderBy = q => q.OrderByDescending(t => t.CreatedAt);
            }
            else
            {
                orderBy = q => q.OrderBy(t => t.DueDate);
            }

            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) &&
                (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)),
                orderBy,
                "");

            if (!tasks.Any())
            {
                return NotFound();
            }

            if (tasks.Count() < filters.ItemsPerPageCount)
            {
                filters.ItemsPerPageCount = tasks.Count();
            }

            var results = tasks.Take(filters.ItemsPerPageCount).Select(t => 
            new
            {
                t.Title,
                DueDate = includeColumns.Contains("DueDate") ? t.DueDate : (DateTime?)null,
                CreatedAt = includeColumns.Contains("CreatedAt") ? t.CreatedAt : (DateTime?)null
            }).ToArray();

            return Ok(results);
        }

        [HttpGet("GetFilteredShortWithComments")]
        public async Task<ActionResult<IEnumerable<Task>>> GetFilteredShortWithComments([FromQuery] MyItemFilter filters)
        {
            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) &&
                (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)),
                q => q.OrderByDescending(t => t.TaskComments.Select(tc => tc.CreatedAt)),
                "TaskComments,Goal");

            if (!tasks.Any())
            {
                return NotFound();
            }

            if (tasks.Count() < filters.ItemsPerPageCount)
            {
                filters.ItemsPerPageCount = tasks.Count();
            }

            var results = tasks.Take(filters.ItemsPerPageCount).Select(t =>
            new
            {
                t.Title,
                Goal = t.Goal.Title,
                Comments = t.TaskComments.Select(tс => new { tс.Comment, tс.CreatedAt }).ToArray()

            }).ToArray();

            return Ok(results);
        }

        [HttpGet("GetFilteredFull")]
        public async Task<ActionResult<IEnumerable<Task>>> GetFilteredFull([FromQuery] MyItemFilter filters)
        {
            IEnumerable<Task> tasks = await _taskRepository.GetFilteredAsync(
                t => (filters.PriorityIds == null || filters.PriorityIds.Contains(t.PriorityId)) &&
                (filters.StatusIds == null || filters.StatusIds.Contains(t.StatusId)) &&
                (filters.GoalIds == null || filters.GoalIds.Contains(t.GoalId.ToString())) &&
                (filters.CategoryIds == null || filters.CategoryIds.Contains(t.GoalId.ToString())),
                q => q.OrderBy(t => t.DueDate),
                "TaskComments,Goal,Priority,Status");

            if (!tasks.Any()) 
            {
                return NotFound();
            }

            if (tasks.Count() < filters.ItemsPerPageCount ) 
            {
                filters.ItemsPerPageCount = tasks.Count(); 
            }

            var results = tasks.Take(filters.ItemsPerPageCount).Select(t =>
            new
            {
                t.Title,
                t.Description,
                Goal = t.Goal.Title,
                Category = t.Goal.Category.Title,
                Comments = t.TaskComments.Select(tс => new { tс.Comment, tс.CreatedAt }).ToArray(),
                Priority = t.Priority.Title,
                Status = t.Status.Title,
                t.CreatedAt,
                t.DueDate
            }).ToArray();

            return Ok(results);
        }
    }
}
