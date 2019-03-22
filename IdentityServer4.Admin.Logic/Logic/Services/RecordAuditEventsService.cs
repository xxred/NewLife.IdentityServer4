





using IdentityServer4.Admin.Logic.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RSK.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class RecordAuditEventsService : IRecordAuditEventsService
  {
    private readonly IRecordAuditableActions audit;
    private readonly IHttpContextAccessor httpContext;
    private readonly ILogger<RecordAuditEventsService> logger;

    public RecordAuditEventsService(AuditProviderFactory auditProvider, IHttpContextAccessor httpContext, ILogger<RecordAuditEventsService> logger)
    {
      this.audit = auditProvider.CreateAuditSource("AdminUI");
      this.httpContext = httpContext;
      this.logger = logger;
    }

    public Task LastRecordContinuationTask { get; private set; }

    public void RecordSuccess(string action, string resourceType, string resourceIdentifier, string resourceName, FormattedString description)
    {
      this.LastRecordContinuationTask = this.audit.RecordSuccess((IAuditEventArguments) new AuditEventArguments(this.GetResourceActor(), action, new AuditableResource(resourceType, resourceIdentifier, resourceName), description)).ContinueWith((Action<Task>) (x => this.LogFailedAudit(action, resourceType, resourceIdentifier, (Exception) x.Exception)), TaskContinuationOptions.OnlyOnFaulted);
    }

    public void RecordFailure(string action, string resourceType, string resourceName, string resourceIdentifier)
    {
      this.LastRecordContinuationTask = this.audit.RecordFailure((IAuditEventArguments) new AuditEventArguments(this.GetResourceActor(), action, new AuditableResource(resourceType, resourceIdentifier, resourceName), new FormattedString(AuditDescriptions.FailureUnknown, Array.Empty<object>()))).ContinueWith((Action<Task>) (x => this.LogFailedAudit(action, resourceType, resourceIdentifier, (Exception) x.Exception)), TaskContinuationOptions.OnlyOnFaulted);
    }

    public void RecordFailure(string action, string resourceType, string resourceName, string resourceIdentifier, FormattedString description)
    {
      this.LastRecordContinuationTask = this.audit.RecordFailure((IAuditEventArguments) new AuditEventArguments(this.GetResourceActor(), action, new AuditableResource(resourceType, resourceIdentifier, resourceName), description)).ContinueWith((Action<Task>) (x => this.LogFailedAudit(action, resourceType, resourceIdentifier, (Exception) x.Exception)), TaskContinuationOptions.OnlyOnFaulted);
    }

    public void RecordUnauthorizedFailure(string action, string resourceType, string resourceName, string resourceIdentifier)
    {
      this.LastRecordContinuationTask = this.audit.RecordFailure((IAuditEventArguments) new AuditEventArguments(this.GetResourceActor(), action, new AuditableResource(resourceType, resourceIdentifier, resourceName), new FormattedString(AuditDescriptions.FailureUnauthorized, new object[1]
      {
        (object) this.GetResourceActor().DisplayName
      }))).ContinueWith((Action<Task>) (x => this.LogFailedAudit(action, resourceType, resourceIdentifier, (Exception) x.Exception)), TaskContinuationOptions.OnlyOnFaulted);
    }

    public ResourceActor GetResourceActor()
    {
      List<Claim> list = this.httpContext.HttpContext.User.Claims.ToList<Claim>();
      return list.Any<Claim>((Func<Claim, bool>) (x => x.Type == "sub")) ? (ResourceActor) new UserResourceActor(list.FirstOrDefault<Claim>((Func<Claim, bool>) (x => x.Type == "sub"))?.Value, list.FirstOrDefault<Claim>((Func<Claim, bool>) (x => x.Type == "name"))?.Value ?? list.FirstOrDefault<Claim>((Func<Claim, bool>) (x => x.Type == "sub"))?.Value) : (ResourceActor) new MachineResourceActor(list.FirstOrDefault<Claim>((Func<Claim, bool>) (x => x.Type == "client_id"))?.Value, list.FirstOrDefault<Claim>((Func<Claim, bool>) (x => x.Type == "client_id"))?.Value);
    }

    private void LogFailedAudit(string action, string resourceType, string resourceIdentifier, Exception e)
    {
      this.logger.LogError(e, "Audit Event Failed: Actor: " + this.GetResourceActor().Identifier + ", Action: " + action + ", ResourceType: " + resourceType + ", Resource: " + resourceIdentifier, Array.Empty<object>());
    }
  }
}
