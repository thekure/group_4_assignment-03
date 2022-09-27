namespace Assignment3.Entities.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Entities;
using Assignment3.Core;

public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;
    public TagRepositoryTests () {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        _context = context;
        _repository = new TagRepository(_context);
    }
   
    [Fact]
    public void Create_Tag_returns_Created_with_TagId()
    {
        var (response, created) = _repository.Create(new TagCreateDTO("A tag"));
       
        response.Should().Be(Response.Created);
       
        created.Should().Be(0);
    }
}
