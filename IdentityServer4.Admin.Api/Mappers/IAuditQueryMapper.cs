
using IdentityServer4.Admin.Api.Controllers;
using IdentityExpress.Manager.BusinessLogic.Logic.Services.AuditQueries;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Admin.Api.Mappers
{
  public interface IAuditQueryMapper
  {
    IAuditQueryMapper AddFiltering(AuditQueryArguments arguments);

    IAuditQueryMapper AddPaging(AuditQueryArguments arguments);

    bool IsValid { get; }

    AuditQuery Query { get; }

    ObjectResult Error { get; }
  }
}
