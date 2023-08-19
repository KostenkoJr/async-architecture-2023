using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskTracker.Data.Context;
using TaskTracker.Data.Entities;
using TaskTracker.Dto;
using TaskTracker.Events.Domain;
using TaskTracker.Events.Stream;
using TaskTracker.Settings;
using Task = TaskTracker.Data.Entities.Task;

namespace TaskTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly KafkaSettings _kafkaSettings;

    public TasksController(IOptions<KafkaSettings> kafkaSettings)
    {
        _kafkaSettings = kafkaSettings.Value;
    }

    [Authorize]
    [HttpGet("all-tasks")]
    public ActionResult<IReadOnlyCollection<Task>> ListAllTasks()
    {
        var userRole = User.Claims.FirstOrDefault(x => x.Type == "Role");
        var canViewAllTasks = userRole?.Value == "Admin";

        if (!canViewAllTasks)
        {
            return new ForbidResult();
        }

        using var db = new TaskTrackerDbContext();
        return Ok(db.Tasks.ToList());
    }
    
    [Authorize]
    [HttpGet("assigned-to-me-tasks")]
    public ActionResult<IReadOnlyCollection<Task>> ListAssignedToMeTasks()
    {
        var currentUserId = Guid.Parse(User.Claims.First(x => x.Type == "Id").Value);
        
        using var db = new TaskTrackerDbContext();
        return Ok(db.Tasks.Where(t => t.Id == currentUserId).ToList());
    }
    
    [Authorize]
    [HttpPost("tasks")]
    public ActionResult<Task> CreateTask(CreateTaskDto inputTask)
    {
        var currentUserId = Guid.Parse(User.Claims.First(x => x.Type == "Id").Value);
        using var db = new TaskTrackerDbContext();
        
        var task = new Task
        {
            PublicId = Guid.NewGuid(),
            Title = inputTask.Title,
            Description = inputTask.Description,
            Cost = Random.Shared.Next(10, 21),
            Award = Random.Shared.Next(20, 40),
            Status = Status.InProgress,
            AuthorId = currentUserId,
            AssigneeId = inputTask.AssigneeId,
        };
        
        db.Tasks.Add(task);
        db.SaveChanges();

        ProduceTaskCreatedEvent(task);
        return Ok(task);
    }
    
    [Authorize]
    [HttpPost("complete-task")]
    public ActionResult<Task> CompleteTask(Guid taskId)
    {
        var currentUserId = Guid.Parse(User.Claims.First(x => x.Type == "Id").Value);
        using var db = new TaskTrackerDbContext();
        
        var task = db.Tasks.Find(taskId);
        if (task is null)
        {
            return NotFound();
        }

        if (currentUserId != task.AssigneeId)
        {
            throw new InvalidOperationException();
        }

        if (task.Status != Status.InProgress)
        {
            throw new InvalidOperationException();
        }

        task.Status = Status.Completed;
        db.SaveChanges();
        
        ProduceTaskCompletedEvent(task);
        
        return Ok(task);
    }
    
    [Authorize]
    [HttpPost("shuffle-tasks")]
    public ActionResult ShuffleTasks()
    {
        var userRole = User.Claims.FirstOrDefault(x => x.Type == "Role");
        var canReassignTasks = userRole?.Value == "Manager" || userRole?.Value == "Admin";

        if (!canReassignTasks)
        {
            return new ForbidResult();
        }
        
        using var db = new TaskTrackerDbContext();
        
        var tasks = db.Tasks.Where(t => t.Status == Status.InProgress).ToList();
        var workers = db.Users.Where(u => u.Role == Role.Worker).ToList();
        var workersCount = workers.Count;
        
        if (!workers.Any())
        {
            return Ok();
        }
        
        foreach (var task in tasks)
        {
            task.AssigneeId = workers[Random.Shared.Next(0, workersCount)].Id;
            db.SaveChanges();
            ProduceTaskAssignEvent(task);
        }
        
        return Ok();
    }
    
    private void ProduceTaskCreatedEvent(Task task)
    {
        var producerConfig = new ProducerConfig { BootstrapServers = _kafkaSettings.BootstrapServers };
        var producer = new ProducerBuilder<string, string>(producerConfig).Build();
        
        producer.Produce("tasks-stream", new Message<string, string>
        {
            Key = task.Id.ToString(), 
            Value = JsonSerializer.Serialize(new TaskCreatedEventData
            {
                PublicId = task.PublicId,
                Title = task.Title,
                Description = task.Description,
                Cost = task.Cost,
                Award = task.Award,
                PublicAuthorId = task.PublicAuthorId,
                PublicAssigneeId = task.PublicAssigneeId
            })
        });
        
        producer.Produce("tasks", new Message<string, string>
        {
            Key = task.Id.ToString(), 
            Value = JsonSerializer.Serialize(new TaskCreatedEventData
            {
                PublicId = task.PublicId,
                Title = task.Title,
                Description = task.Description,
                Cost = task.Cost,
                Award = task.Award,
                PublicAuthorId = task.PublicAuthorId,
                PublicAssigneeId = task.PublicAssigneeId
            })
        });
    }
    
    private void ProduceTaskAssignEvent(Task task)
    {
        var producerConfig = new ProducerConfig { BootstrapServers = _kafkaSettings.BootstrapServers };
        var producer = new ProducerBuilder<string, string>(producerConfig).Build();
        
        producer.Produce("tasks", new Message<string, string>
        {
            Key = task.Id.ToString(), 
            Value = JsonSerializer.Serialize(new TaskAssignedEventData
            {
                PublicId = task.PublicId,
                PublicAssigneeId = task.PublicAssigneeId
            })
        });
    }
    
    private void ProduceTaskCompletedEvent(Task task)
    {
        var producerConfig = new ProducerConfig { BootstrapServers = _kafkaSettings.BootstrapServers };
        var producer = new ProducerBuilder<string, string>(producerConfig).Build();
        
        producer.Produce("tasks-lifecycle", new Message<string, string>
        {
            Key = task.Id.ToString(), 
            Value = JsonSerializer.Serialize(new TaskCompletedEventData
            {
                PublicId = task.PublicId,
                PublicAssigneeId = task.PublicAssigneeId
            })
        });
    }
}