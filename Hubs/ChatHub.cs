using EternalBAND.Data;
using EternalBAND.Models;
using EternalBAND.Models.ViewModel;
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
}