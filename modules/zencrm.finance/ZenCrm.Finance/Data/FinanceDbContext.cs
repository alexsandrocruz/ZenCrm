using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ZenCrm.Finance.Data;

[ConnectionStringName(FinanceDbProperties.ConnectionStringName)]
public class FinanceDbContext : AbpDbContext<FinanceDbContext>, IFinanceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFinance();
    }
}
