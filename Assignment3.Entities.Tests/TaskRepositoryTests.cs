namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
private readonly KanbanContext _context;
    private readonly TagRepository _tagRepository;
    private readonly TaskRepository _taskRepository;
    private readonly UserRepository _userRepository;
    
    public TaskRepositoryTests () {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        
        _context = context;
        _taskRepository = new TaskRepository(_context);
        _tagRepository = new TagRepository(_context);
        _userRepository = new UserRepository(_context);
    }


    // [Fact]
    // public void getAssignedUserName_Should()
    // {
    //     var (response, created) = _repository.Create(new TaskCreateDTO("taskTitle", null, null, new List<string>()));
       
    //     response.Should().Be(Response.Created);
       
    //     created.Should().Be(0);
    // }

    


}
