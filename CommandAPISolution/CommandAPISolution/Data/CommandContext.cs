using CommandAPISolution.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandAPISolution.Data;

public class CommandContext:DbContext
{
    public CommandContext()
    {
    }

    public CommandContext(DbContextOptions<CommandContext> options) : base(options)
    {
    }
    public DbSet<Command> CommandItems {get; set;}
}