
var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);

Console.WriteLine("done");