using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserRepository
    {
        Task<Result> Authenticate(AuthenticateRequest model);
        Task<Result> GetAllUsers();
        Task<Result> GetUserById(Guid id);
        Task<Result> CreateUser(User model);
    }
}
