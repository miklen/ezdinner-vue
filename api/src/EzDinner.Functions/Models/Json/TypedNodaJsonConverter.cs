using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Json
{
    /// <summary>
    /// https://tech.corlife.com/2020/02/nodatime-serialization-output-in-azure-functions.html
    /// </summary>
    public abstract class TypedNodaJsonConverter : JsonConverter
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected JsonConverter NodaConverter { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public override bool CanConvert(Type objectType)
        {
            return NodaConverter.CanConvert(objectType);
        }

        public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
        {
            return NodaConverter.ReadJson(
            reader,
            objectType,
            existingValue,
            serializer);
        }

        public override void WriteJson(
        JsonWriter writer,
        object value,
        JsonSerializer serializer)
        {
            NodaConverter.WriteJson(writer, value, serializer);
        }
    }
}
