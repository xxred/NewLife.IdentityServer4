





using System.Data;
using System.Data.Common;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  internal static class DbExtensions
  {
    public static DbCommand AddParameter(this DbCommand cmd, string name, DbType type, object value = null, ParameterDirection direction = ParameterDirection.Input)
    {
      DbParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = name;
      parameter.DbType = type;
      parameter.Direction = direction;
      if (direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput)
        parameter.Value = value;
      cmd.Parameters.Add((object) parameter);
      return cmd;
    }

    public static string GetStringOrNull(this DbDataReader reader, int ordinal)
    {
      if (reader.IsDBNull(ordinal))
        return (string) null;
      return reader.GetString(ordinal);
    }

    public static T? GetValueOrNull<T>(this DbDataReader reader, int ordinal) where T : struct
    {
      if (reader.IsDBNull(ordinal))
        return new T?();
      return new T?(reader.GetFieldValue<T>(ordinal));
    }
  }
}
