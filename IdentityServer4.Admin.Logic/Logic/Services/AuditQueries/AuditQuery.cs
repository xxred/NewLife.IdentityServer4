





using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Logic.Services.AuditQueries
{
  public class AuditQuery
  {
    public AuditQuery(DateTime from, DateTime to)
    {
      this.From = from;
      this.To = to;
      this.PageSize = 25;
    }

    public DateTime From { get; }

    public DateTime To { get; }

    public string SubjectId { get; set; }

    public string ResourceType { get; set; }

    public string Resource { get; set; }

    public string Action { get; set; }

    public string Subject { get; set; }

    public string Source { get; set; }

    public bool? Success { get; set; }

    public int? PageNumber { get; set; }

    public int PageSize { get; set; }

    public bool OrderDescending { get; set; }

    public string GetSuccessString()
    {
      return !this.Success.HasValue ? "all" : (this.Success.Value ? "successful" : "unsuccessful");
    }

    public override bool Equals(object obj)
    {
      AuditQuery auditQuery = obj as AuditQuery;
      return auditQuery != null && this.From == auditQuery.From && (this.To == auditQuery.To && this.SubjectId == auditQuery.SubjectId) && (this.ResourceType == auditQuery.ResourceType && this.Resource == auditQuery.Resource && (this.Action == auditQuery.Action && this.Subject == auditQuery.Subject)) && (this.Source == auditQuery.Source && EqualityComparer<bool?>.Default.Equals(this.Success, auditQuery.Success) && (EqualityComparer<int?>.Default.Equals(this.PageNumber, auditQuery.PageNumber) && this.PageSize == auditQuery.PageSize)) && this.OrderDescending == auditQuery.OrderDescending;
    }

    public override int GetHashCode()
    {
      return (((((((((((-1712231146 * -1521134295 + this.From.GetHashCode()) * -1521134295 + this.To.GetHashCode()) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SubjectId)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ResourceType)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Resource)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Action)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Subject)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Source)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.Success)) * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.PageNumber)) * -1521134295 + this.PageSize.GetHashCode()) * -1521134295 + this.OrderDescending.GetHashCode();
    }

    public override string ToString()
    {
      string str = string.Format("from {0} to {1}", (object) this.From, (object) this.To);
      if (this.Subject == null && this.Source == null && this.Action == null && this.Resource == null)
        return str;
      Dictionary<string, string> source = new Dictionary<string, string>()
      {
        {
          "Subject",
          this.Subject
        },
        {
          "Source",
          this.Source
        },
        {
          "Action",
          this.Action
        },
        {
          "Resource",
          this.Resource
        }
      };
      return str + " with filters: " + string.Join(", ", source.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (param => !string.IsNullOrEmpty(param.Value))).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Key + ": '" + x.Value + "'")));
    }
  }
}
