namespace TableFlowBackend.Hubs;

using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    public async Task NotifyCustomer(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
