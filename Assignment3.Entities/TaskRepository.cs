using Assignment3.Core;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;

    public TaskRepository(KanbanContext context){
        _context = context;
    }

    // string Title, int? AssignedToId, string? Description, ICollection<string> Tags
    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        // var entity = _context.Tasks.FirstOrDefault(t => t.Title == task.Title);
        // Response response;



        // if (entity is null){
        //     task.Tags

        //     entity = new Task(){Title = task.Title, Description = task.Description, Tags = task.Tags};

        //     _context.Tasks.Add(entity);
        //     _context.SaveChanges();

        //     response = Response.Created;
        // } else {
        //     response = Response.Conflict;
        // }

        // return (response, entity.Id);
        throw new NotImplementedException();
    }

    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }

    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }

    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
    }
}
