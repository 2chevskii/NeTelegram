namespace NeTelegram.Commands;

public record CommandDefinition(string Name, string Description, Type HandlerType);
