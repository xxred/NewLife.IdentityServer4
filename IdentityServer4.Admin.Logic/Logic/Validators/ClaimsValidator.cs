





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Validators
{
  public class ClaimsValidator : IValidator<IList<string>>
  {
    private readonly IIdentityUnitOfWorkFactory factory;

    public ClaimsValidator(IIdentityUnitOfWorkFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      this.factory = factory;
    }

    public async Task<IdentityResult> Validate(IList<string> claimTypes)
    {
      
      
      ClaimsValidator.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20 = new ClaimsValidator.\u003C\u003Ec__DisplayClass2_0();
      
      cDisplayClass20.claimTypes = claimTypes;
      
      
      if (cDisplayClass20.claimTypes == null || !cDisplayClass20.claimTypes.Any<string>())
        return IdentityResult.Success;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        
        
        ClaimsValidator.\u003C\u003Ec__DisplayClass2_1 cDisplayClass21 = new ClaimsValidator.\u003C\u003Ec__DisplayClass2_1();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        
        
        
        IEnumerable<IdentityExpressClaimType> source = await uow.ClaimTypeRepository.Find(Expression.Lambda<Func<IdentityExpressClaimType, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
        {
          cDisplayClass20.claimTypes,
          (Expression) Expression.Lambda<Func<string, bool>>((Expression) Expression.Equal(y, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityExpressClaimType.Name)))), new ParameterExpression[1]
          {
            parameterExpression2
          })
        }), new ParameterExpression[1]
        {
          parameterExpression1
        }));
        
        cDisplayClass21.matchingClaimTypes = source.ToList<IdentityExpressClaimType>;
        source = (IEnumerable<IdentityExpressClaimType>) null;
        
        
        
        
        return cDisplayClass21.matchingClaimTypes.Count == cDisplayClass20.claimTypes.Distinct<string>().Count<string>() ? IdentityResult.Success : IdentityResult.Failed(cDisplayClass20.claimTypes.Where<string>(new Func<string, bool>(cDisplayClass21.\u003CValidate\u003Eb__1)).Select<string, IdentityError>((Func<string, IdentityError>) (x => new IdentityError()
        {
          Description = "Could not find ClaimType of '" + x + "'"
        })).ToArray<IdentityError>());
      }
    }
  }
}
