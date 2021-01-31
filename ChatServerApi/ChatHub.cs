using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System;
using Interfaces;
using Entities.Models;

namespace ChatServerApi
{
    public class ChatHub : Hub<IChatHub>
    {
        private IChatRepository _genericService;
        public ChatHub(IChatRepository genericService)
        {
            _genericService = genericService;
        }
        public async Task SendMessageAsync(ChatMessage chat)
        {
			try
			{
                if (chat.ChatId == 0)
                {
                    Chat msg = new Chat();
                    msg.Message = chat.Text;
                    msg.ReceiverId = chat.ReceiverId;
                    msg.SenderId = chat.SenderId;
                    Result result = await _genericService.Save(msg);
                    if (result.IsSuccess)
                    {
                        chat.IsChnaged = false;
                        chat.ChatId = Convert.ToInt64(((string[])result.Data)[0]);
                        chat.Time = ((string[])result.Data)[1];
                        await Clients.All.MessageReceivedFromHub(chat);
                        //await Clients.User(chat.ReceiverId.ToString()).MessageReceivedFromHub(chat);
                    }
                }
				else
				{
                    chat.IsChnaged = true;
                    await Clients.All.MessageReceivedFromHub(chat);
                }
            }
			catch (Exception ex)
			{

			}
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.NewUserConnected("a new user connectd");
        }
	}

   
}
