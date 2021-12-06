using EzDinner.Core.Aggregates.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Query.Core.FamilyQueries
{
    public class FamilyQueryService : IFamilyQueryService
    {
        private readonly IFamilyQueryRepository _familyRepository;
        private readonly IUserRepository _userRepository;

        public FamilyQueryService(IFamilyQueryRepository familyRepository, IUserRepository userRepository)
        {
            _familyRepository = familyRepository;
            _userRepository = userRepository;
        }

        public async Task<FamilyDetails?> GetFamilyDetailsAsync(Guid familyId)
        {
            var family = await _familyRepository.GetFamilyDetailsAsync(familyId);
            if (family is null) return null;
            ResolveFamilyMemberNames(new List<FamilyDetails>() { family });
            return family;
        }

        public async Task<IEnumerable<FamilyDetails>> GetFamiliesDetailsAsync(Guid userId)
        {
            var families = await _familyRepository.GetFamiliesDetailsAsync(userId);
            ResolveFamilyMemberNames(families);
            return families;
        }

        private void ResolveFamilyMemberNames(IEnumerable<FamilyDetails> families)
        {
            var familyMemberNames = GetUserNames(families);
            foreach (var familyMember in families.SelectMany(s => s.FamilyMembers))
            {
                // family members without autonomy has their names stored in local db and not in B2C
                if (!familyMember.HasAutonomy) continue;
                familyMember.Name = familyMemberNames[familyMember.Id];
            }
        }

        private Dictionary<Guid, string> GetUserNames(IEnumerable<FamilyDetails> families)
        {
            // N+1 microservice problem... TODO solve by saving necessary information closer to usage or get list of users in one request
            var ownerIds = families.Select(s => s.OwnerId).Distinct();
            var memberIds = families.SelectMany(s => s.FamilyMembers).Select(s => s.Id).Distinct();
            var tasks = new List<Task<User>>();
            foreach (var userId in ownerIds.Union(memberIds))
            {
                tasks.Add(_userRepository.GetUser(userId));
            }
            Task.WaitAll(tasks.ToArray());
            var users = tasks.ToDictionary(k => k.Result.Id, v => v.Result.FullName);
            return users;
        }
    }
}
