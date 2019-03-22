using System;
using System.Runtime.Serialization;

namespace IdentityServer4.Admin.Api.Policy
{
	public class PolicyConcurrencyException : Exception
	{
		public PolicyConcurrencyException()
		{
		}

		public PolicyConcurrencyException(string message)
			: base(message)
		{
		}

		public PolicyConcurrencyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected PolicyConcurrencyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
