using System.IO;
using System.Threading.Tasks;
using EzDinner.Functions.Models.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;

namespace EzDinner.Functions
{
    public class DinnerAddMenuItem
    {
        private readonly ILogger<DinnerAddMenuItem> _logger;

        public DinnerAddMenuItem(ILogger<DinnerAddMenuItem> logger)
        {
            _logger = logger;
        }
        
        [FunctionName(nameof(DinnerAddMenuItem))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "dinner/menuitem")] HttpRequest req
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var menuItem = await req.GetBodyAs<DinnerAddMenuItemCommandModel>();

            _logger.LogInformation($"Adding dish: {menuItem.DishId} to date: {menuItem.Date}");

            return new OkResult();
        }
    }
}

