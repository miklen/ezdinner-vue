using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Core.Aggregates.FamilyMemberAggregate
{
    public interface IFamilyMemberRepository
    {
        Task<FamilyMember> GetFamilyMember(Guid id);
    }
}
