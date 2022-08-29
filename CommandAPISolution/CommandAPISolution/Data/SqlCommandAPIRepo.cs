using CommandAPISolution.Models;

namespace CommandAPISolution.Data;

public class SqlCommandAPIRepo:ICommandAPIRepo
{
    private readonly CommandContext context;

    public SqlCommandAPIRepo(CommandContext context)
    {
        this.context = context;
    }

    public bool SaveChanges()
    {
        int saveChanges = context.SaveChanges();
        return saveChanges >= 0;
    }

    public IEnumerable<Command> GetAllCommands()
    {
        return this.context.CommandItems;
    }

    public Command GetCommandById(int id)
    {
        return this.context.CommandItems.FirstOrDefault(cmd=>cmd.Id==id);
    }

    public void CreateCommand(Command cmd)
    {
        if (cmd == null)
        {
            throw new ArgumentNullException(nameof(cmd));
        }

        context.CommandItems.Add(cmd);
    }

    public void UpdateCommand(Command cmd)
    {
        
    }

    public void DeleteCommand(Command cmd)
    {
        if(cmd == null)
        {
            throw new ArgumentNullException(nameof(cmd));
        }
        context.CommandItems.Remove(cmd);
    }
}