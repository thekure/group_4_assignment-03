namespace Assignment3.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Assignment3.Entities;



public sealed class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }
    

    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();



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

        modelBuilder.Entity<User>()
                    .HasIndex(c => c.Email).IsUnique();
        
        modelBuilder.Entity<User>()
                    .Property(c => c.Name)
                    .HasMaxLength(50);
       
       modelBuilder
            .Entity<Task>()
            .HasOne(e => e.User)
            .WithMany(e => e.Tasks);
     

    }

}
