





using IdentityServer4.Admin.Logic.Logic.Services.AuditQueries;
using RSK.Audit;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
  public interface IQueryAuditedEventsService
  {
    Task<IAuditQueryResult> ExecuteQuery(AuditQuery query);
  }
}
