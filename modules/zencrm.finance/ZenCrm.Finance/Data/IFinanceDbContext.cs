using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ZenCrm.Finance.Data;

[ConnectionStringName(FinanceDbProperties.ConnectionStringName)]
public interface IFinanceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
