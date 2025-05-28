namespace NeTelegram.Commands;

public class CommandAttribute(string name) : Attribute
{
    public string Name => name;
}
