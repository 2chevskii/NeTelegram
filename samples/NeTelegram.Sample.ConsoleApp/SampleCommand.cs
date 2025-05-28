using NeTelegram.Commands;

[Command("sample")]
class SampleCommand : CommandBase
{
    public override Task Invoke()
    {
        Console.WriteLine("Sample command invoked: {0}", Context.Update.Message.From.Username);
        return Task.CompletedTask;
    }
}