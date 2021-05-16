using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Core.Aggregates.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid id);
        Task<User?> GetUser(string email);
    }
}
