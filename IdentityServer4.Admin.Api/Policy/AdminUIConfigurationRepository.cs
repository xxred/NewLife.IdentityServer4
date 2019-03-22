using IdentityExpress.Manager.BusinessLogic.Entities.IdentityServer;
using IdentityExpress.Manager.BusinessLogic.Logic.IdentityServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
	public class AdminUIConfigurationRepository : IAdminUIConfigurationRepository
	{
		private readonly ExtendedConfigurationDbContext context;

		public AdminUIConfigurationRepository(ExtendedConfigurationDbContext context)
		{
			this.context = context;
		}

		public async Task<string> TryGetValue(string key, string defaultValue)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			ConfigurationEntry configEntry = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ConfigurationEntry>((IQueryable<ConfigurationEntry>)context.ConfigurationEntries, (Expression<Func<ConfigurationEntry, bool>>)((ConfigurationEntry x) => x.Key == key), default(CancellationToken));
			if (configEntry != null)
			{
				return configEntry.Value;
			}
			return defaultValue;
		}

		public async Task SetValue(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ConfigurationEntry configEntry = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ConfigurationEntry>((IQueryable<ConfigurationEntry>)context.ConfigurationEntries, (Expression<Func<ConfigurationEntry, bool>>)((ConfigurationEntry x) => x.Key == key), default(CancellationToken));
			if (configEntry == null)
			{
				DbSet<ConfigurationEntry> configurationEntries = context.ConfigurationEntries;
				ConfigurationEntry val = new ConfigurationEntry();
				val.set_Key(key);
				val.set_Value(value);
				configurationEntries.Add(val);
				await context.SaveChangesAsync(default(CancellationToken));
			}
			else
			{
				configEntry.set_Value(value);
				context.ConfigurationEntries.Update(configEntry);
				await context.SaveChangesAsync(default(CancellationToken));
			}
		}
	}
}
