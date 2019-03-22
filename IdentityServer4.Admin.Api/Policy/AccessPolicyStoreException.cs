using System;
using System.Runtime.Serialization;

namespace IdentityServer4.Admin.Api.Policy
{
	public class AccessPolicyStoreException : Exception
	{
		public AccessPolicyStoreException()
		{
		}

		public AccessPolicyStoreException(string message)
			: base(message)
		{
		}

		public AccessPolicyStoreException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected AccessPolicyStoreException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
