





using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Entities
{
  public class PagedResult<T>
  {
    public IEnumerable<T> Results { get; set; }

    public int CurrentPage { get; set; }

    public int PageCount { get; set; }

    public int PageSize { get; set; }

    public long TotalCount { get; set; }

    public bool IsSorted { get; set; }

    public static PagedResult<T> Single(T result)
    {
      PagedResult<T> pagedResult = new PagedResult<T>();
      pagedResult.CurrentPage = 1;
      pagedResult.PageCount = 1;
      pagedResult.PageSize = 1;
      pagedResult.TotalCount = 1L;
      List<T> objList;
      if ((object) result == null)
      {
        objList = new List<T>();
      }
      else
      {
        objList = new List<T>();
        objList.Add(result);
      }
      pagedResult.Results = (IEnumerable<T>) objList;
      return pagedResult;
    }
  }
}
