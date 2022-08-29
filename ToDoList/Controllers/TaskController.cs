using DataAccessLibrary;
using DataAccessLibrary.Dtos;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<FullTaskDto>> GetTasksAsync()
        {
            var tasks = _taskRepository.GetTasks().Select(t => t.AsDto());
            return tasks;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FullTaskDto>> GetTaskAsync(Guid id)
        {
            var task = _taskRepository.GetTask(id);

            if (task is null)
            {
                return NotFound();
            }

            return task.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTaskAsync(CreateNewTaskDto taskDto)
        {
            await _taskRepository.CreateNewTaskAsync(taskDto.Name, taskDto.Description);
            return NoContent();
        }

        [HttpPut("Start/{id}")]
        public async Task<ActionResult> StartTaskAsync(Guid id)
        {
            if (!await _taskRepository.TryStartTaskAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("Complete/{id}")]
        public async Task<ActionResult> CompleteTaskAsync(Guid id)
        {
            if (!await _taskRepository.TryCompleteTaskAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateTaskAsync(TaskDto taskDto)
        {
            bool found = false;

            if (taskDto.Name is not null)
            {
                found = await _taskRepository.TryUpdateTaskNameAsync(taskDto.Id, taskDto.Name);
            }
            if (taskDto.Description is not null)
            {
                found = await _taskRepository.TryUpdateTaskDescriptionAsync(taskDto.Id, taskDto.Description);
            }
            if (found is false)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskAsync(Guid id)
        {
            if (!await _taskRepository.TryDeleteTaskAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }



        //private readonly IOrderRepository repository;

        //public OrderController(IOrderRepository repository)
        //{
        //    this.repository = repository;
        //}

        //// GET /Order/
        //[HttpGet]
        //public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        //{
        //    var order = (await repository.GetOrdersAsync())
        //                .Select(order => order.AsDto());

        //    return order;
        //}

        //// GET /Order/name
        //[HttpGet("{name}")]
        //public async Task<ActionResult<OrderDto>> GetOrderAsync(string name)
        //{
        //    var order = await repository.GetOrderAsync(name);

        //    if (order is null)
        //    {
        //        return NotFound();
        //    }

        //    return order.AsDto();
        //}

        //// PUT /Order/
        //[HttpPut("{id}")]
        //public async Task<ActionResult> CompleteOrderAsync(CompleteOrderDto orderDto)
        //{
        //    await repository.CompleteOrderAsync(orderDto.Id);
        //    return NoContent();
        //}

        //// Post /Order/
        //[HttpPost]
        //public async Task<ActionResult> CreateOrderAsync(CreateOrderDto orderDto)
        //{
        //    Order order = new()
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = orderDto.Name,
        //        Instruction = orderDto.Instruction,
        //        IsCompleted = false
        //    };

        //    await repository.CreateOrderAsync(order);
        //    return NoContent();
        //}

        //// Delete /Order/
        //[HttpDelete]
        //public async Task<ActionResult> DeleteAllOrdersAsync()
        //{
        //    await repository.DeleteAllOrdersAsync();
        //    return NoContent();
        //}

        //// Delete /Order/{id}
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteOrderAsync(Guid id)
        //{
        //    var existingItem = await repository.GetOrderAsync(id);

        //    if (existingItem is null)
        //    {
        //        return NotFound();
        //    }

        //    await repository.DeleteOrderAsync(id);

        //    return NoContent();
        //}
    }
}