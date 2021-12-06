using EzDinner.Core.Aggregates.FamilyAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Query.Core.FamilyQueries
{
    public interface IFamilyQueryRepository
    {
        Task<IEnumerable<FamilyDetails>> GetFamiliesDetailsAsync(Guid userId);
        Task<FamilyDetails?> GetFamilyDetailsAsync(Guid familyId);
    }
}
