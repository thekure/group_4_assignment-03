namespace Assignment3.Entities;

public sealed class KanbanContext : DbContext
{
    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();

    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options){
            
        }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
        .Entity<Task>()
        .Property(e => e.State)
        .HasConversion(
            v => v.ToString(),
            v => (State)Enum.Parse(typeof(State), v));

        modelBuilder
        .Entity<Task>()
        .HasMany<Tag>(e => e.Tags)
        .WithMany(e => e.Tasks);

    }
}

