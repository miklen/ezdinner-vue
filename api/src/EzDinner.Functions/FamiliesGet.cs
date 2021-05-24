using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Core.Aggregates.UserAggregate;
using EzDinner.Functions.Models.Query;
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
  public class FamiliesGet
  {
    private readonly ILogger<FamiliesGet> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IFamilyRepository _familyRepository;
    private readonly IMapper _mapper;

    public FamiliesGet(ILogger<FamiliesGet> logger, IMapper mapper, IUserRepository userRepository, IFamilyRepository familyRepository)
    {
      _logger = logger;
      _userRepository = userRepository;
      _familyRepository = familyRepository;
      _mapper = mapper;
    }

    [FunctionName(nameof(FamiliesGet))]
    [RequiredScope("backendapi")]
    public async Task<IActionResult?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "families/select")] HttpRequest req)
    {
      _logger.LogInformation("C# HTTP trigger function processed a request.");

      var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
      if (!authenticationStatus) return authenticationResponse;

      var userId = Guid.Parse(req.HttpContext.User.GetNameIdentifierId() ?? "");
      var families = await _familyRepository.GetFamilySelectorsAsync(userId);

      var familieyQueryModels = families.Select(_mapper.Map<FamilySelectQueryModel>);
      return new OkObjectResult(families);
    }
  }
}

