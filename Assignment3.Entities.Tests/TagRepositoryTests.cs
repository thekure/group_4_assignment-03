namespace Assignment3.Entities.Tests;


public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _taskRepository;
    private readonly TagRepository _repository;
    private readonly UserRepository _userRepository;

    public TagRepositoryTests () {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        _context = context;
        _repository = new TagRepository(_context);
        _taskRepository = new TaskRepository(_context);
        _userRepository = new UserRepository(_context);
    }
   
    [Fact]
    public void Create_Tag_returns_Created_with_TagId()
    {
        var (response, created) = _repository.Create(new TagCreateDTO("A tag"));
       
        response.Should().Be(Response.Created);
       
        created.Should().Be(0);
    }

    [Fact]
    public void Create_Tag_returns_Conflict_with_TagId()
    {
        var (response, created) = _repository.Create(new TagCreateDTO("A tag"));
        var (response1, created1) = _repository.Create(new TagCreateDTO("A tag"));
       
        response1.Should().Be(Response.Conflict);
       
        created1.Should().Be(0);
    }

    [Fact]
    public void Delete_Tag_returns_response_Not_Found()
    {

        var response = _repository.Delete(0,false);
       
        response.Should().Be(Response.NotFound);
    }

    [Fact]
    public void Delete_Tag_returns_Conflict()
    {
        var testList = new List<string>();
        testList.Add("test");

        var (userResponse, id) = _userRepository.Create(new UserCreateDTO("name", "hans@gmail.com"));
        var (test, created) = _repository.Create(new TagCreateDTO("test"));
        var (testTask, testId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, testList));

        var response = _repository.Delete(0);

        response.Should().Be(Response.Conflict);
    
    }

[Fact]
    public void Delete_Tag_returns_Removed()
    {
        var (test, created) = _repository.Create(new TagCreateDTO("A tag"));
        var response = _repository.Delete(0);
       
        response.Should().Be(Response.Deleted);
    }


    [Fact]
    public void Read_Tag_returns_Tag()
    {
        _repository.Create(new TagCreateDTO("A tag"));
       
        var response = _repository.Read(0);
        var result = new TagDTO(0, "A tag");

        response.Should().Be(result);
    }

    [Fact]
    public void Read_Tag_update_returns_Response_NotFound()
    {
        var testTag = new TagUpdateDTO(0, "A tag");
        
        var response = _repository.Update(testTag);

        response.Should().Be(Response.NotFound);
    }


    /*
        Cant succesfully test the following endpoint due to issues with TasksRepository 
        the following test should be correct
    */
    [Fact]
    public void Read_Tag_update_returns_Response_Conflict()
    {
        var testList = new List<string>();
        testList.Add("test");

        var (userResponse, id) = _userRepository.Create(new UserCreateDTO("name", "hans@gmail.com"));
        var (test, created) = _repository.Create(new TagCreateDTO("test"));
        var (testTask, testId) = _taskRepository.Create(new TaskCreateDTO ("testTask", 0, null, testList));

        var testTag = new TagUpdateDTO(0, "A tag");
        var tag = new Tag{ Id = 1,
                        Name = "A tag"};

        _context.Add(tag);
        _context.SaveChanges();
        
        var response = _repository.Update(testTag);

        response.Should().Be(Response.Conflict);
    }


    [Fact]
    public void Read_Tag_update_returns_Response_Updated()
    {
        var (testResponse, created) = _repository.Create(new TagCreateDTO("A tag"));
        var testTag = new TagUpdateDTO(0, "A tag");
        
        var response = _repository.Update(testTag);

        response.Should().Be(Response.Updated);
    }

}
