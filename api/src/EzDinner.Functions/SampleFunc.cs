using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Threading.Tasks;

namespace EzDinner.Functions
{
    public class SampleFunc
    {
        private readonly ILogger<SampleFunc> _logger;
        public SampleFunc(ILogger<SampleFunc> logger)
        {
            _logger = logger;
        }

        [FunctionName("SampleFunc")]
        [RequiredScope("backendapi")] // The Azure Function will only accept tokens 1) for users, and 2) having the "backendapi" scope for this API
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) =
                await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            string name = req.HttpContext.User.Identity.IsAuthenticated ? req.HttpContext.User.GetDisplayName() : null;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new JsonResult(responseMessage);
        }

    }
}
