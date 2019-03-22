using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Api.Models
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; }

        public static ErrorDto FromIdentityResult(IdentityResult result)
        {
            if (result?.Errors == null || !result.Errors.Any<IdentityError>())
                return (ErrorDto)null;
            return new ErrorDto()
            {
                Errors = result.Errors.Select<IdentityError, string>((Func<IdentityError, string>)(x => x.Description)).ToList<string>()
            };
        }

        public static ErrorDto FromModelState(ModelStateDictionary result)
        {
            if (result == null || !result.Any<KeyValuePair<string, ModelStateEntry>>() || result.All<KeyValuePair<string, ModelStateEntry>>((Func<KeyValuePair<string, ModelStateEntry>, bool>)(x => x.Value == null)))
                return (ErrorDto)null;
            return new ErrorDto()
            {
                Errors = result.Values.SelectMany<ModelStateEntry, ModelError>((Func<ModelStateEntry, IEnumerable<ModelError>>)(x => (IEnumerable<ModelError>)x.Errors)).Select<ModelError, string>((Func<ModelError, string>)(x => x.ErrorMessage)).ToList<string>()
            };
        }
    }
}
