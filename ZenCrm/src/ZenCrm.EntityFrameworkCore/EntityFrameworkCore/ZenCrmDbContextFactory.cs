using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ZenCrm.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ZenCrmDbContextFactory : IDesignTimeDbContextFactory<ZenCrmDbContext>
{
    public ZenCrmDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        ZenCrmEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ZenCrmDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new ZenCrmDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ZenCrm.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
