using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using sifoodproject.Areas.Users.Hubs;
using sifoodproject.Models;
using sifoodproject.Services;

namespace sifoodproject.Areas.Admin.NewFolder
{
    public class AdminChatHub:Hub
    {
        private readonly IHubContext<AdminChatHub> _adminchathubContext;
        private readonly IHubContext<ChatHub> _chathubContext;
        public AdminChatHub(IHubContext<AdminChatHub> adminchathubContext, IHubContext<ChatHub> chathubContext)
        {
            _adminchathubContext = adminchathubContext;
            _chathubContext = chathubContext;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await AddToGroup("U018");
            await _chathubContext.Clients.Group("U018").SendAsync("ConnectMessage", "admin已上線");
            await _adminchathubContext.Clients.Group("U018").SendAsync("ConnectMessage", "admin已上線");
        }
        //離線事件
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            
            // 更新聊天內容
            await _chathubContext.Clients.Group("U018").SendAsync("DisConnectMessage", "admin已離線" );
            await _adminchathubContext.Clients.Group("U018").SendAsync("DisConnectMessage", "admin已離線");
            await base.OnDisconnectedAsync(ex);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string user, string message)
        {
            await _chathubContext.Clients.Group("U018").SendAsync("ReceiveMessage", user, message);
            await _adminchathubContext.Clients.Group("U018").SendAsync("ReceiveMessage", user, message);
        }
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
