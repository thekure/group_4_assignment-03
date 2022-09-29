using Assignment3.Core;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;

    public TaskRepository(KanbanContext context){
        _context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var entity = _context.Tasks.FirstOrDefault(t => t.Title == task.Title);
        Response response;



        if (entity is null){
            var convertedTags = new List<Tag>();
            User user = null;
            foreach (string s in task.Tags){
                var t = _context.Tags
                    .Where(t => t.Name == s)
                    .FirstOrDefault();
                if(t == null){
                    convertedTags.Add(new Tag(){Name = s});
                } else {
                    convertedTags.Add(t);
                }
            }

           if(task.AssignedToId != null){
                    user = _context.Users
                        .Where(u => u.Id == task.AssignedToId).ToList()[0];
            } 

            entity = new Task(){
                Title = task.Title, 
                Description = task.Description, 
                Tags = convertedTags,
                User = user,
                Created = DateTime.Now,
                State = State.New,
                StateUpdated = DateTime.Now
            };
             
            
            entity.State = State.New;
            

            _context.Tasks.Add(entity);
            _context.SaveChanges();

            response = Response.Created;
        } else {
            response = Response.Conflict;
        }
        return (response, entity.Id);
    }

    public Response Delete(int taskId)
    {
        var task = _context.Tasks.Include(t => t.Tags).FirstOrDefault(t => t.Id == taskId);
        Response response;

        if (task is null){
            response = Response.NotFound;
        } else {
            switch(task.State){
                case State.New:
                    _context.Tasks.Remove(task);
                    response = Response.Deleted;
                    break;

                case State.Active:
                    task.State = State.Removed;
                    task.StateUpdated = DateTime.Now;
                    response = Response.Deleted;
                    break;

                case State.Resolved:
                case State.Closed:
                case State.Removed:
                    response = Response.Conflict;
                    break;  

                default: 
                    response = Response.BadRequest;
                    break;
            }
        }
         _context.SaveChanges();
        return response;
    }

    public TaskDetailsDTO Read(int taskId)
    {                   
        var tasks = from t in _context.Tasks
                    where t.Id == taskId
                    select new TaskDetailsDTO(
                        t.Id, 
                        t.Title, 
                        t.Description, 
                        t.Created, 
                        t.User.Name, 
                        t.Tags.Select(tg => tg.Name).ToList().AsReadOnly(), 
                        t.State,
                        t.StateUpdated
                    );

        return tasks.FirstOrDefault();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tasks = from t in _context.Tasks
                    orderby t.Title
                    select new TaskDTO(
                        t.Id, 
                        t.Title, 
                        t.User.Name,
                        t.Tags.Select(tg => tg.Name).ToList().AsReadOnly(), 
                        t.State
                    );

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        var tasks = from t in _context.Tasks
                    where t.State == state
                    select new TaskDTO(
                        t.Id, 
                        t.Title, 
                        t.User.Name, 
                        t.Tags.Select(tg => tg.Name).ToList().AsReadOnly(), 
                        t.State
                    );

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
         var tasks = from t in _context.Tasks
                    where t.Tags.Select(tg => tg.Name).ToList().Contains(tag)
                    select new TaskDTO(
                        t.Id, 
                        t.Title, 
                        t.User.Name,
                        t.Tags.Select(tg => tg.Name).ToList().AsReadOnly(), 
                        t.State
                    );

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
         var tasks = from t in _context.Tasks
                    where t.AssignedToId == userId
                    select new TaskDTO(
                        t.Id, 
                        t.Title, 
                        t.User.Name, 
                        t.Tags.Select(tg => tg.Name).ToList().AsReadOnly(), 
                        t.State
                    );

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
         var tasks = from t in _context.Tasks
                    where t.State == State.Removed
                    orderby t.Title
                    select new TaskDTO(
                        t.Id, 
                        t.Title, 
                        t.User.Name,
                        t.Tags.Select(tg => tg.Name).ToList().AsReadOnly(), 
                        t.State
                    );
        return tasks.ToArray();
    }

    public Response Update(TaskUpdateDTO task)
    {
         var entity = _context.Tasks.Include(t => t.Tags).FirstOrDefault(t => t.Id == task.Id);
        Response response;

        if (entity is null)
        {
            response = Response.NotFound;

        }
        else if (_context.Tasks.FirstOrDefault(t => t.Id != task.Id && t.Title == task.Title) != null)
        {
            response = Response.Conflict;
        }
        else
        {
            var convertedTags = new List<Tag>();
            foreach (string s in task.Tags){
                var t = _context.Tags
                    .Where(t => t.Name == s)
                    .FirstOrDefault();
                if(t == null){
                    convertedTags.Add(new Tag(){Name = s});
                } else {
                    convertedTags.Add(t);
                }
            }

            entity.Id = task.Id;
            entity.Title = task.Title;
            entity.Description = task.Description;

            if(task.AssignedToId == null){
                response = Response.BadRequest;
            } else {
                 entity.UserId = (int) task.AssignedToId;
            }
            
            entity.State = task.State;
            entity.StateUpdated = DateTime.Now;
            entity.Tags = convertedTags;
            _context.SaveChanges();

            response = Response.Updated;
        }

        return response;
    }
    
}
