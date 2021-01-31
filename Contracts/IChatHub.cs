using Entities.Models;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IChatHub
    {
        Task MessageReceivedFromHub(ChatMessage message);
        Task NewUserConnected(string message);
    }
}
