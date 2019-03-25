
using IdentityServer4.Admin.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Formatters
{
  public class CsvOutputFormatter : TextOutputFormatter
  {
    public CsvOutputFormatter()
    {
      this.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse((StringSegment) "text/csv"));
      this.SupportedEncodings.Add(Encoding.UTF8);
      this.SupportedEncodings.Add(Encoding.Unicode);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
      HttpResponse response = context.HttpContext.Response;

      string filename = CsvOutputFormatter.GenerateAuditQueryResultsFilename(context.HttpContext.Request);
      response.Headers.Add("Content-Disposition", (StringValues) ("attachment; filename=" + filename));
      await response.WriteAsync("When,Subject Name,Subject Identifier,Subject Type,Action,Resource Name,Resource Type,Source,Succeeded,Description\n", new CancellationToken());

      filename = (string) null;
    }

    private static string GenerateAuditQueryResultsFilename(HttpRequest request)
    {
      IQueryCollection query = request.Query;
      string str1 = (string) query["From"];
      string str2 = (string) query["To"];
      string[] strArray = request.Path.Value.Split('/', StringSplitOptions.None);
      string str3 = "";
      if (((IEnumerable<string>) strArray).Contains<string>("users"))
        str3 = "user:" + strArray[2] + "_";
      else if (((IEnumerable<string>) strArray).Contains<string>("clients"))
        str3 = "client:" + strArray[2] + "_";
      return "adminui.audits_" + str3 + str1 + "_to_" + str2 + ".csv";
    }
  }
}
