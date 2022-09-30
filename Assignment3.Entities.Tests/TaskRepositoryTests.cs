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

    #pragma warning disable

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
    public void Delete_Should_Return_Conflict()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var updateResponse = _taskRepository.Update(new TaskUpdateDTO(1, "testTask", 0, null, tasksListOfTags, State.Resolved));
        var testResponse = _taskRepository.Delete(1);

        // Assert
        testResponse.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Read_Should_Return_testTask()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var testTask = new TaskDetailsDTO(1, "testTask", null, DateTime.Now, "HansGruber", tasksListOfTags, State.New, DateTime.Now);
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var testTaskDetailsDTO = _taskRepository.Read(1);

        // Assert
        Assert.Equal(testTask.Created, testTaskDetailsDTO.Created, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(testTask.StateUpdated, testTaskDetailsDTO.StateUpdated, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(testTask.AssignedToName, "HansGruber");
        Assert.Equal(testTask.Title, "testTask");
    }
    
    [Fact]
    public void ReadAll_Should_Return_testTask()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var testTask = new TaskDTO(1, "testTask", "HansGruber", tasksListOfTags, State.New);
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var testTaskDTO = _taskRepository.ReadAll().ToList()[0];

        // Assert
        Assert.Equal(testTask.Tags, testTaskDTO.Tags.ToList());
        Assert.Equal(testTask.Id, testTaskDTO.Id);
        Assert.Equal(testTask.Title, testTaskDTO.Title);
        Assert.Equal(testTask.State, testTaskDTO.State);
    }
    
    [Fact]
    public void ReadAllByState_Should_Return_testTask()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var testTask = new TaskDTO(1, "testTask", "HansGruber", tasksListOfTags, State.New);
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var testTaskDTO = _taskRepository.ReadAllByState(State.New).ToList()[0];

        // Assert
        Assert.Equal(testTask.Tags, testTaskDTO.Tags.ToList());
        Assert.Equal(testTask.Id, testTaskDTO.Id);
        Assert.Equal(testTask.Title, testTaskDTO.Title);
        Assert.Equal(testTask.State, testTaskDTO.State);
    }
    
    [Fact]
    public void ReadAllByTag_Should_Return_testTask()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var testTask = new TaskDTO(1, "testTask", "HansGruber", tasksListOfTags, State.New);
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var testTaskDTO = _taskRepository.ReadAllByTag("testTag").ToList()[0];

        // Assert
        Assert.Equal(testTask.Tags, testTaskDTO.Tags.ToList());
        Assert.Equal(testTask.Id, testTaskDTO.Id);
        Assert.Equal(testTask.Title, testTaskDTO.Title);
        Assert.Equal(testTask.State, testTaskDTO.State);
    }
    
    [Fact]
    public void ReadAllByUser_Should_Return_testTask()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var testTask = new TaskDTO(1, "testTask", "HansGruber", tasksListOfTags, State.New);
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var testTaskDTO = _taskRepository.ReadAllByUser(0).ToList()[0];

        // Assert
        Assert.Equal(testTask.Tags, testTaskDTO.Tags.ToList());
        Assert.Equal(testTask.Id, testTaskDTO.Id);
        Assert.Equal(testTask.Title, testTaskDTO.Title);
        Assert.Equal(testTask.State, testTaskDTO.State);
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

    [Fact]
    public void Update_Should_Return_NotFound()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};

        // Act
        var updateResponse = _taskRepository.Update(new TaskUpdateDTO(1, "testTask", 0, null, tasksListOfTags, State.Active));

        // Assert
        updateResponse.Should().Be(Response.NotFound);
    }

    
    public void Update_Should_Return_Conflict()
    {
        // This is handled by create. The scenario can't happen.
    }
    

    [Fact]
    public void Update_Should_Return_Updated()
    {
        // Arrange 
        var tasksListOfTags = new List<string>{"testTag"};
        var removedTasks = new List<TaskDTO>{new TaskDTO(1, "testTask", "HansGruber", tasksListOfTags, State.Removed)};
        var (userResponse, userId) = _userRepository.Create(new UserCreateDTO("HansGruber", "hansgruber@gmail.com"));
        // var (tagResponse, tagId) = _tagRepository.Create(new TagCreateDTO("testTag"));
        var (taskResponse, taskId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, tasksListOfTags));

        // Act
        var updateResponse = _taskRepository.Update(new TaskUpdateDTO(1, "testTask", 0, null, tasksListOfTags, State.Active));

        // Assert
        updateResponse.Should().Be(Response.Updated);
    }


}
