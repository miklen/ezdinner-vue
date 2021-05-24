using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Authorization
{
    public interface IPermissionService
    {
        Task CreateOwnerRole(Guid familyId);
        Task AssignRoleToUser(Guid userId, string role, Guid familyId);
    }
}
