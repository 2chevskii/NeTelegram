namespace NeTelegram.Commands;

public class CommandAttribute(string name, string description = "Default command description")
    : Attribute
{
    public string Name => name;

    public string Description => description;
}
