using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql;

namespace Assignment3;

internal class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
{
    public KanbanContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=localhost;Username=postgres;Password=s$cret;Database=BDSA";

        using var con = new NpgsqlConnection(connectionString);
        con.Open();

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new KanbanContext(optionsBuilder.Options);
    }
}

