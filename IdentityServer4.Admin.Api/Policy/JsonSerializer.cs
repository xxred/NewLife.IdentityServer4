using Newtonsoft.Json;

namespace IdentityServer4.Admin.Api.Policy
{
	public class JsonSerializer<T> : ISerializer<T>
	{
		public string Serialize(T value)
		{
			return JsonConvert.SerializeObject((object)value);
		}

		public T Deserialize(string stringValue)
		{
			return JsonConvert.DeserializeObject<T>(stringValue);
		}
	}
}
