





using Microsoft.AspNetCore.Http;
using RSK.Audit;
using System;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class PreventRecordingOfReadEvents : IRecordAuditEventsService
  {
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IRecordAuditEventsService recordAuditEventsService;

    public PreventRecordingOfReadEvents(IHttpContextAccessor httpContextAccessor, IRecordAuditEventsService recordAuditEventsService)
    {
      if (httpContextAccessor == null)
        throw new ArgumentNullException(nameof (httpContextAccessor));
      if (recordAuditEventsService == null)
        throw new ArgumentNullException(nameof (recordAuditEventsService));
      this.httpContextAccessor = httpContextAccessor;
      this.recordAuditEventsService = recordAuditEventsService;
    }

    public void RecordSuccess(string action, string resourceType, string resourceIdentifier, string resourceName, FormattedString description)
    {
      if (this.IsHttpGet())
        return;
      this.recordAuditEventsService.RecordSuccess(action, resourceType, resourceIdentifier, resourceName, description);
    }

    public void RecordFailure(string action, string resourceType, string resourceIdentifier, string resourceName)
    {
      if (this.IsHttpGet())
        return;
      this.recordAuditEventsService.RecordFailure(action, resourceType, resourceIdentifier, resourceName);
    }

    public void RecordUnauthorizedFailure(string action, string resourceType, string resourceIdentifier, string resourceName)
    {
      this.recordAuditEventsService.RecordUnauthorizedFailure(action, resourceType, resourceIdentifier, resourceName);
    }

    public void RecordFailure(string action, string resourceType, string resourceIdentifier, string resourceName, FormattedString description)
    {
      if (this.IsHttpGet())
        return;
      this.recordAuditEventsService.RecordFailure(action, resourceType, resourceIdentifier, resourceName, description);
    }

    public ResourceActor GetResourceActor()
    {
      return this.recordAuditEventsService.GetResourceActor();
    }

    private bool IsHttpGet()
    {
      return this.httpContextAccessor.HttpContext.Request.Method == "GET";
    }
  }
}
