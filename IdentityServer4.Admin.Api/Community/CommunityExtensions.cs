
using Microsoft.AspNetCore.Authentication;
using System;

namespace IdentityServer4.Admin.Api.Community
{
  public static class CommunityExtensions
  {
    public static AuthenticationBuilder AddCommunity(this AuthenticationBuilder builder, Action<CommunityOptions> options)
    {
      return builder.AddScheme<CommunityOptions, CommunityHandler>("Community", "Community", options);
    }
  }
}
