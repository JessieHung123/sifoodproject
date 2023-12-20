using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using sifoodproject.Areas.Admin.NewFolder;
using sifoodproject.Models;
using sifoodproject.Services;

namespace sifoodproject.Areas.Users.Hubs
{

    public class ChatHub: Hub
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IHubContext<AdminChatHub> _adminchathubContext;
        private readonly IUserIdentityService _userIdentityService;

       public ChatHub(IHubContext<ChatHub> hubContext,IUserIdentityService userIdentityService,IHubContext<AdminChatHub> adminchathubContext)
        {
            _hubContext = hubContext;
            _userIdentityService = userIdentityService;
            _adminchathubContext = adminchathubContext;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var userId= _userIdentityService.GetUserId();
            await AddToGroup(userId);
            await _hubContext.Clients.Group(userId).SendAsync("ConnectMessage", "user已上線");
            await _adminchathubContext.Clients.Group(userId).SendAsync("ConnectMessage", "user已上線");
        }
        //離線事件
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
            var userId = _userIdentityService.GetUserId();
            // 更新聊天內容
            await _hubContext.Clients.Group(userId).SendAsync("DisConnectMessage", "user已離線");
            await _adminchathubContext.Clients.Group(userId).SendAsync("DisConnectMessage", "user已離線");
        }
        public async Task AddToGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        public async Task SendMessage(string user, string message)
        {
            var userId = _userIdentityService.GetUserId();
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveMessage", user, message);
            await _adminchathubContext.Clients.Group(userId).SendAsync("ReceiveMessage", user, message);
        }


    }
}
