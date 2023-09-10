using EternalBAND.Data;
using Microsoft.AspNetCore.SignalR;

namespace EternalBAND.Hubs;

public class ChatHub:Hub
{
    private ApplicationDbContext _context;
    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task SendMessage(string username, string message)
    {
      await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}