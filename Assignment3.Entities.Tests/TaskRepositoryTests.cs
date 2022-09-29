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


    [Fact]
    public void Create_Should_Return_Created_and_0()
    {
        // Arrange
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        // var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var (testResponse, testId) = _taskRepository.Create(new TaskCreateDTO("taskTitle", 0, null, new List<string>()));

        // Assert
        testResponse.Should().Be(Response.Created);
        testId.Should().Be(1);
    }

    [Fact]
    public void Create_Should_Return_Conflict()
    {
        // Arrange
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        // var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var (testResponse0, testId0) = _taskRepository.Create(new TaskCreateDTO("taskTitle", 0, null, new List<string>()));
        var (testResponse, testId) = _taskRepository.Create(new TaskCreateDTO("taskTitle", 0, null, new List<string>()));

        // Assert
        testResponse.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Delete_Should_Return_Deleted()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var updateResponse = _taskRepository.Update(new TaskUpdateDTO(1, "testTask", 0, null, tasksListOfTags, State.Active));
        var testResponse = _taskRepository.Delete(1);

        // Assert
        testResponse.Should().Be(Response.Deleted);
    }

    [Fact]
    public void Delete_Should_Return_NotFound()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        // var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var testResponse = _taskRepository.Delete(1);

        // Assert
        testResponse.Should().Be(Response.NotFound);
    }



    [Fact]
    public void ReadAllRemoved_Should_Return_testTask()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var removedTasks = new List<TaskDTO>{new TaskDTO(1, "testTask", "HansGruber", tasksListOfTags, State.Removed)};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var updateResponse = _taskRepository.Update(new TaskUpdateDTO(1, "testTask", 0, null, tasksListOfTags, State.Active));
        var testResponse = _taskRepository.Delete(1);
        var testRemovedTasks = _taskRepository.ReadAllRemoved();

        // Assert
        testRemovedTasks.Should().BeEquivalentTo(removedTasks);
    }

 

    


}
