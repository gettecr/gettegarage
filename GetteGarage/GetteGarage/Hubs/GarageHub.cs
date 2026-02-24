using Microsoft.AspNetCore.SignalR;


namespace GetteGarage.Hubs
{
    public class GarageHub : Hub
    {
        // Static variable so it persists across all connections
        private static int _onlineUsers = 0;

        public override async Task OnConnectedAsync()
        {
            Interlocked.Increment(ref _onlineUsers);
            
            // Broadcast the new count to everyone
            await Clients.All.SendAsync("UpdateUserCount", _onlineUsers);
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            Interlocked.Decrement(ref _onlineUsers);
            
            // Broadcast the new count to everyone
            await Clients.All.SendAsync("UpdateUserCount", _onlineUsers);
            
            // FIX: Pass 'ex' into the base method
            await base.OnDisconnectedAsync(ex); 
        }
    }
}