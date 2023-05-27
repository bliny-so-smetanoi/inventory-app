
using InventoryApp.Models;
using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace InventoryApp.Hubs
{
    public class ClassroomHub : Hub
    {
        private readonly IMemoryCache _memoryCache;
        public ClassroomHub(IMemoryCache cache) {
            _memoryCache = cache;
        }
        public async Task SendReload(string classroomId)
        {
            List<string>? list;

            if(_memoryCache.TryGetValue(classroomId,out list))
            {
                await Clients.Clients(list).SendAsync("Reload");
            }   
        }

        public override async Task OnConnectedAsync()
        {
            var contextHttp = Context.GetHttpContext();
            var classroom = contextHttp.Request.Query["classroom"].ToString();
            var connectionId = Context.ConnectionId;

            List<string>? value;

            if(_memoryCache.TryGetValue(classroom, out value))
            {
                _memoryCache.Remove(classroom);
                value?.Add(connectionId);
                _memoryCache.Set(classroom, value);
            }
            else
            {
                var list = new List<string>
                {
                    connectionId
                };
                _memoryCache.Set(classroom, list);
            }
            

            Console.WriteLine("After adding:" + ((List<string>)_memoryCache.Get(classroom)).Count );
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var contextHttp = Context.GetHttpContext();
            var classroom = contextHttp.Request.Query["classroom"].ToString();
            var connectionId = Context.ConnectionId;
            List<string>? value;

            if (_memoryCache.TryGetValue(classroom, out value))
            {
                _memoryCache.Remove(classroom);
                value?.Remove(connectionId);
                _memoryCache.Set(classroom, value);
            }

            Console.WriteLine("ushel");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
