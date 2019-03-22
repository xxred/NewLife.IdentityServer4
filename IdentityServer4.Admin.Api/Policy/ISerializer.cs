namespace IdentityServer4.Admin.Api.Policy
{
	public interface ISerializer<T>
	{
		string Serialize(T value);

		T Deserialize(string stringValue);
	}
}
