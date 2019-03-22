





using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
  public interface IWebhookService
  {
    Task<bool> PostToEndpoint(string endpointUrl, string email);
  }
}
