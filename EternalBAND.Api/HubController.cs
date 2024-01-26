using EternalBAND.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.Api
{
    public class HubController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public HubController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastMessage(string broadCastTitle, object arg)
        {
            await _hubContext.Clients.All.SendAsync(broadCastTitle, arg);
        }
    }
}
