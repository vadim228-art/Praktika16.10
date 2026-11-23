using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task Send(string userName, string message)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await Clients.All.SendAsync("Receive", userName, message);
    }
}
