
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Reflection;

namespace IdentityServer4.Admin.Api.Hosting
{
  public static class DbContextOptionsFactory<TContext> where TContext : DbContext
  {
    private static readonly string MigrationAssembly = typeof (Startup).GetTypeInfo().Assembly.GetName().Name;

    public static DbContextOptions<TContext> CreateSqlite(string connectionString)
    {
      DbContextOptionsBuilder<TContext> contextOptionsBuilder = new DbContextOptionsBuilder<TContext>();
      SqliteDbContextOptionsBuilderExtensions.UseSqlite<TContext>((DbContextOptionsBuilder<M0>) contextOptionsBuilder, connectionString, (Action<SqliteDbContextOptionsBuilder>) (options => ((RelationalDbContextOptionsBuilder<SqliteDbContextOptionsBuilder, SqliteOptionsExtension>) options).MigrationsAssembly(DbContextOptionsFactory<TContext>.MigrationAssembly)));
      return contextOptionsBuilder.Options;
    }

    public static DbContextOptions<TContext> CreateSqlServer(string connectionString)
    {
      DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
      optionsBuilder.UseSqlServer<TContext>(connectionString, (Action<SqlServerDbContextOptionsBuilder>) (options => options.MigrationsAssembly(DbContextOptionsFactory<TContext>.MigrationAssembly)));
      return optionsBuilder.Options;
    }

    public static DbContextOptions<TContext> CreateMySql(string connectionString)
    {
      DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
      optionsBuilder.UseMySql<TContext>(connectionString, (Action<MySqlDbContextOptionsBuilder>) (options => options.MigrationsAssembly(DbContextOptionsFactory<TContext>.MigrationAssembly)));
      return optionsBuilder.Options;
    }

    public static DbContextOptions<TContext> CreatePostgreSql(string connectionString)
    {
      DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
      optionsBuilder.UseNpgsql<TContext>(connectionString, (Action<NpgsqlDbContextOptionsBuilder>) (options => options.MigrationsAssembly(DbContextOptionsFactory<TContext>.MigrationAssembly)));
      return optionsBuilder.Options;
    }
  }
}
