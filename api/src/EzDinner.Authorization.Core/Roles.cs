using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Authorization.Core
{
    public static class Roles
    {
        public static readonly string Owner = nameof(Owner);
        public static readonly string FamilyMember = nameof(FamilyMember);
    }
}
