





using IdentityServer4.Admin.Logic.Interfaces.Services;
using RSK.Audit;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services.AuditQueries
{
  public class QueryAuditedEventService : IQueryAuditedEventsService
  {
    private readonly AuditProviderFactory auditProviderFactory;

    public QueryAuditedEventService(AuditProviderFactory auditProviderFactory)
    {
      if (auditProviderFactory == null)
        throw new ArgumentNullException(nameof (auditProviderFactory));
      this.auditProviderFactory = auditProviderFactory;
    }

    public Task<IAuditQueryResult> ExecuteQuery(AuditQuery queryArguments)
    {
      if (queryArguments == null)
        throw new ArgumentNullException(nameof (queryArguments));
      QueryAuditedEventService.ValidateQuery(queryArguments);
      IAuditQuery baseQuery = this.CreateBaseQuery(queryArguments);
      IAuditQuery auditQuery = baseQuery.WithStringQueryArgument(queryArguments.Resource, new Func<Matches, string, IAuditQuery>(baseQuery.AndResource)).WithStringQueryArgument(queryArguments.ResourceType, new Func<Matches, string, IAuditQuery>(baseQuery.AndResourceType)).WithStringQueryArgument(queryArguments.Action, new Func<Matches, string, IAuditQuery>(baseQuery.AndAction)).WithStringQueryArgument(queryArguments.Subject, new Func<Matches, string, IAuditQuery>(baseQuery.AndSubject)).WithStringQueryArgument(queryArguments.Source, new Func<Matches, string, IAuditQuery>(baseQuery.AndSource));
      bool? success = queryArguments.Success;
      if (success.HasValue)
      {
        success = queryArguments.Success;
        auditQuery = success.Value ? auditQuery.AndActionSucceeded() : auditQuery.AndActionFailed();
      }
      return queryArguments.OrderDescending ? auditQuery.ExecuteDescending(AuditQuerySort.When) : auditQuery.ExecuteAscending(AuditQuerySort.When);
    }

    private IAuditQuery CreateBaseQuery(AuditQuery queryArguments)
    {
      IQueryableAuditableActions auditQuery = this.auditProviderFactory.CreateAuditQuery();
      return queryArguments.PageNumber.HasValue ? auditQuery.Between(queryArguments.From, queryArguments.To, queryArguments.PageNumber.Value, queryArguments.PageSize) : auditQuery.Between(queryArguments.From, queryArguments.To);
    }

    private static void ValidateQuery(AuditQuery query)
    {
      if (query.From > query.To)
        throw new AuditQueryValidationException("From is after to");
    }
  }
}
