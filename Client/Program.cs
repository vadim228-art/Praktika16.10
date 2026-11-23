using Microsoft.AspNetCore.SignalR.Client;

Console.Write("Введите ваше имя: ");
var userName = Console.ReadLine()?.Trim();

while (string.IsNullOrWhiteSpace(userName))
{
    Console.Write("Имя не может быть пустым. Введите имя: ");
    userName = Console.ReadLine()?.Trim();
}

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5192/chat")
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("Receive", (senderName, message) =>
{
    Console.WriteLine();
    Console.WriteLine($"[{senderName}]: {message}");
    Console.Write("> ");
});

try
{
    await connection.StartAsync();
    Console.WriteLine($"Вы вошли в чат под именем {userName}.");
    Console.WriteLine("Введите сообщение. Для выхода напишите /exit.");

    while (true)
    {
        Console.Write("> ");
        var message = Console.ReadLine();

        if (message is null || message.Trim().Equals("/exit", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            continue;
        }

        await connection.InvokeAsync("Send", userName, message.Trim());
    }
}
catch (Exception exception)
{
    Console.WriteLine($"Не удалось подключиться к серверу: {exception.Message}");
}
finally
{
    await connection.DisposeAsync();
}
