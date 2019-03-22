





namespace IdentityServer4.Admin.Logic.Entities.Services
{
  public class Pagination
  {
    public Pagination()
    {
    }

    public Pagination(int page, int pageSize)
    {
      this.Page = page;
      this.PageSize = pageSize;
    }

    public int Page { get; set; }

    public int PageSize { get; set; }
  }
}
