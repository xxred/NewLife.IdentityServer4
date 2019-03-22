





namespace IdentityServer4.Admin.Logic.Interfaces.Mappers
{
  public interface IMapper<in TIn, out TOut>
  {
    TOut Map(TIn input);
  }
}
