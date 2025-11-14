using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ZenCrm.EventFlow.Data;

[ConnectionStringName(EventFlowDbProperties.ConnectionStringName)]
public interface IEventFlowDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
