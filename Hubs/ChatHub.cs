using EternalBAND.Data;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace EternalBAND.Hubs;

public class ChatHub:Hub
{
    private ApplicationDbContext _context;
    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
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