using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace EternalBAND.Api.Hubs;

public class ChatHub : Hub
{
    public ChatHub()
    {
    }
    public async Task SendMessage(Messages message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        await Clients.All.SendAsync("ReceiveMessage", jsonMessage);
    }

    public async Task SendNotification(Notification notif)
    {
        var jsonMessage = JsonSerializer.Serialize(notif);
        await Clients.All.SendAsync("ReceiveNotification", jsonMessage);
    }
}