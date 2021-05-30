using NodaTime.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Json
{
    public class LocalDateConverter : TypedNodaJsonConverter
    {
        public LocalDateConverter()
        {
            NodaConverter = NodaConverters.LocalDateConverter;
        }
    }
}
