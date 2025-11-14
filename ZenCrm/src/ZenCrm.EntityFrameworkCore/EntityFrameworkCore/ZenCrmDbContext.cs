using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using ZenCrm.Books;
using ZenCrm.Sales;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ZenCrm.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ZenCrmDbContext :
    AbpDbContext<ZenCrmDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<Book> Books { get; set; }

    // CRM Sales Entities
    public DbSet<SalesLead> SalesLeads { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Interaction> Interactions { get; set; }
    public DbSet<SalesOpportunity> SalesOpportunities { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public ZenCrmDbContext(DbContextOptions<ZenCrmDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();
        
        builder.Entity<Book>(b =>
        {
            b.ToTable(ZenCrmConsts.DbTablePrefix + "Books",
                ZenCrmConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });

        // Configure CRM Sales Entities
        builder.Entity<SalesLead>(b =>
        {
            b.ToTable(ZenCrmConsts.DbTablePrefix + "SalesLeads", ZenCrmConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FirstName).IsRequired().HasMaxLength(128);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(32);
            b.Property(x => x.MobilePhone).HasMaxLength(32);
            b.Property(x => x.Company).HasMaxLength(512);
            b.Property(x => x.JobTitle).HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(2000);

            b.HasIndex(x => x.Email).IsUnique(false);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.AssignedUserId);
            b.HasIndex(x => x.ClientId);
            b.HasIndex(x => x.Converted);
        });

        builder.Entity<Client>(b =>
        {
            b.ToTable(ZenCrmConsts.DbTablePrefix + "Clients", ZenCrmConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.DocumentNumber).HasMaxLength(32);
            b.Property(x => x.Description).HasMaxLength(512);
            b.Property(x => x.Website).HasMaxLength(256);
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(32);
            b.Property(x => x.Address).HasMaxLength(1024);
            b.Property(x => x.City).HasMaxLength(256);
            b.Property(x => x.State).HasMaxLength(128);
            b.Property(x => x.PostalCode).HasMaxLength(64);
            b.Property(x => x.Country).HasMaxLength(128);

            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.DocumentNumber).IsUnique(false);
            b.HasIndex(x => x.Type);
            b.HasIndex(x => x.Industry);
            b.HasIndex(x => x.AssignedUserId);
        });

        builder.Entity<Customer>(b =>
        {
            b.ToTable(ZenCrmConsts.DbTablePrefix + "Customers", ZenCrmConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FirstName).IsRequired().HasMaxLength(128);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(32);
            b.Property(x => x.MobilePhone).HasMaxLength(32);
            b.Property(x => x.JobTitle).HasMaxLength(128);
            b.Property(x => x.Department).HasMaxLength(256);
            b.Property(x => x.Notes).HasMaxLength(512);

            b.HasIndex(x => x.FirstName);
            b.HasIndex(x => x.LastName);
            b.HasIndex(x => x.Email).IsUnique(false);
            b.HasIndex(x => x.ClientId);
            b.HasIndex(x => x.AssignedUserId);
            b.HasIndex(x => x.IsPrimaryContact);
            b.HasIndex(x => x.IsKeyDecisionMaker);
        });

        builder.Entity<Interaction>(b =>
        {
            b.ToTable(ZenCrmConsts.DbTablePrefix + "Interactions", ZenCrmConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Subject).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.Location).HasMaxLength(512);
            b.Property(x => x.Outcome).HasMaxLength(1024);
            b.Property(x => x.AdditionalData).HasMaxLength(2048);

            b.HasIndex(x => x.ScheduledDate);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Type);
            b.HasIndex(x => x.OwnerUserId);
            b.HasIndex(x => x.SalesLeadId);
            b.HasIndex(x => x.ClientId);
            b.HasIndex(x => x.CustomerId);
        });

        builder.Entity<SalesOpportunity>(b =>
        {
            b.ToTable(ZenCrmConsts.DbTablePrefix + "SalesOpportunities", ZenCrmConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.LostReason).HasMaxLength(2000);
            b.Property(x => x.Competitor).HasMaxLength(512);

            b.HasIndex(x => x.Stage);
            b.HasIndex(x => x.ExpectedCloseDate);
            b.HasIndex(x => x.OwnerUserId);
            b.HasIndex(x => x.SalesLeadId);
            b.HasIndex(x => x.ClientId);
            b.HasIndex(x => x.ParentOpportunityId);
            b.HasIndex(x => x.EstimatedValue);
        });

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ZenCrmConsts.DbTablePrefix + "YourEntities", ZenCrmConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
