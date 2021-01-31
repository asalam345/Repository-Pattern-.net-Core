using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IChatRepository
    {
        Task<Result> Get(Guid senderId, Guid receiverId);
        Task<Result> Save(Chat chat);
        Task<Result> Update(Chat chat);
        Task<Result> Delete(long id);

    }
}
