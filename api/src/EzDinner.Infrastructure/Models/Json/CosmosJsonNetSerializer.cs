using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EzDinner.Infrastructure.Models.Json
{
    /// <summary>
    /// Due to lack of adding Converters to CosmosDbs Json.NET wrapper, we need to supply our own.
    /// 
    /// https://github.com/Azure/azure-cosmos-dotnet-v3/issues/551
    /// </summary>
    public class CosmosJsonNetSerializer : CosmosSerializer
    {
        private static readonly Encoding _defaultEncoding = new UTF8Encoding(false, true);
        private readonly JsonSerializer _serializer;
        private readonly JsonSerializerSettings _serializerSettings;

        public CosmosJsonNetSerializer()
            : this(new JsonSerializerSettings())
        {
        }

        public CosmosJsonNetSerializer(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
            _serializer = JsonSerializer.Create(this._serializerSettings);
        }

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)(stream);
                }

                using (StreamReader sr = new StreamReader(stream))
                {
                    using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
                    {
                        return _serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }
        }

        public override Stream ToStream<T>(T input)
        {
            MemoryStream streamPayload = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(streamPayload, encoding: _defaultEncoding, bufferSize: 1024, leaveOpen: true))
            {
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Newtonsoft.Json.Formatting.None;
                    _serializer.Serialize(writer, input);
                    writer.Flush();
                    streamWriter.Flush();
                }
            }

            streamPayload.Position = 0;
            return streamPayload;
        }
    }
}
