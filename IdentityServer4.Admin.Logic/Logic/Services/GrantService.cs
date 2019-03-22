





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Stores.Serialization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
    public class GrantService : IGrantService
  {
    private readonly IIdentityServerUnitOfWorkFactory factory;
    private readonly IPersistentGrantSerializer serializer;

    public GrantService(IIdentityServerUnitOfWorkFactory factory, IPersistentGrantSerializer serializer)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      if (serializer == null)
        throw new ArgumentNullException(nameof (serializer));
      this.factory = factory;
      this.serializer = serializer;
    }

    public async Task<IdentityResult> RevokeAll(string subject)
    {
      IdentityResult identityResult;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityResult identityResult1 = await uow.PersistedGrantRepository.DeleteBySubject(subject);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        identityResult = result;
      }
      return identityResult;
    }

    public async Task<IdentityResult> RevokeClient(string subject, string clientId)
    {
      IdentityResult identityResult;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityResult identityResult1 = await uow.PersistedGrantRepository.DeleteBySubjectAndClient(subject, clientId);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        identityResult = result;
      }
      return identityResult;
    }

    public async Task<IList<Consent>> GetConsentForSubject(string subject)
    {
      
      
      GrantService.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new GrantService.\u003C\u003Ec__DisplayClass5_0();
      
      cDisplayClass50.subject = subject;
      
      cDisplayClass50.\u003C\u003E4__this = this;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        
        
        GrantService.\u003C\u003Ec__DisplayClass5_1 cDisplayClass51 = new GrantService.\u003C\u003Ec__DisplayClass5_1();
        
        cDisplayClass51.CS\u0024\u003C\u003E8__locals1 = cDisplayClass50;
        
        
        IEnumerable<PersistedGrant> source = await uow.PersistedGrantRepository.Find((Expression<Func<PersistedGrant, bool>>) (x => x.SubjectId == cDisplayClass51.CS\u0024\u003C\u003E8__locals1.subject && x.Type == "user_consent" && (x.Expiration == new DateTime?() || (DateTime?) DateTime.UtcNow < x.Expiration)));
        
        cDisplayClass51.consent = source.ToList<PersistedGrant>();
        source = (IEnumerable<PersistedGrant>) null;
        
        if (!cDisplayClass51.consent.Any<PersistedGrant>())
          return (IList<Consent>) new List<Consent>();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        IEnumerable<IdentityServer4.EntityFramework.Entities.Client> clients = await uow.ClientRepository.Find(Expression.Lambda<Func<IdentityServer4.EntityFramework.Entities.Client, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass51.consent,
          (Expression) Expression.Lambda<Func<PersistedGrant, bool>>((Expression) Expression.Equal(y.ClientId, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityServer4.EntityFramework.Entities.Client.get_ClientId)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        }));
        
        cDisplayClass51.clients = clients;
        clients = (IEnumerable<IdentityServer4.EntityFramework.Entities.Client>) null;
        ParameterExpression parameterExpression3;
        ParameterExpression parameterExpression4;
        
        
        
        IEnumerable<ExtendedClient> extendedClients = await uow.ExtendedClientRepository.Find(Expression.Lambda<Func<ExtendedClient, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass51.consent,
          (Expression) Expression.Lambda<Func<PersistedGrant, bool>>((Expression) Expression.Equal(y.ClientId, (Expression) Expression.Property((Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ExtendedClient.get_ClientId)))), new ParameterExpression[1]
          {
            parameterExpression4
          })
        }), new ParameterExpression[1]
        {
          parameterExpression3
        }));
        
        cDisplayClass51.extendedClients = extendedClients;
        extendedClients = (IEnumerable<ExtendedClient>) null;
        
        
        List<Consent> mappedConsent = cDisplayClass51.consent.Select<PersistedGrant, Consent>(new Func<PersistedGrant, Consent>(cDisplayClass51.\u003CGetConsentForSubject\u003Eb__3)).ToList<Consent>();
        return (IList<Consent>) mappedConsent;
      }
    }
  }
}
