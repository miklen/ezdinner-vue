using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Authorization
{ 
    public static class Actions
    {
        public static readonly string Create = nameof(Create);
        public static readonly string Read = nameof(Read);
        public static readonly string Update = nameof(Update);
        public static readonly string Delete = nameof(Delete);
        public static readonly string All = "*";
    }
}
