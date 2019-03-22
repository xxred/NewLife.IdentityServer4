





using RSK.Audit;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public interface IRecordAuditEventsService
  {
    void RecordSuccess(string action, string resourceType, string resourceIdentifier, string resourceName, FormattedString description);

    void RecordFailure(string action, string resourceType, string resourceIdentifier, string resourceName);

    void RecordUnauthorizedFailure(string action, string resourceType, string resourceIdentifier, string resourceName);

    void RecordFailure(string action, string resourceType, string resourceIdentifier, string resourceName, FormattedString description);

    ResourceActor GetResourceActor();
  }
}
