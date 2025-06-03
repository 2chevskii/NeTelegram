using NeTelegram.Commands;

[Command("test")]
class TestCommand : CommandBase
{
    public override Task Invoke()
    {
        Console.WriteLine("Test command invoked with args: [{0}]", string.Join(", ", Args));
        return Task.CompletedTask;
    }
}