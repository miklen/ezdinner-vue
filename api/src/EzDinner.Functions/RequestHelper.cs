using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Functions
{
    public static class RequestHelper
    {
        public static async Task<T> GetBodyAs<T>(this HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<T>(requestBody);
            return data;
        }
    }
}
