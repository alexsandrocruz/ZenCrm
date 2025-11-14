using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ZenCrm.Juris.Data;

[ConnectionStringName(JurisDbProperties.ConnectionStringName)]
public interface IJurisDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
