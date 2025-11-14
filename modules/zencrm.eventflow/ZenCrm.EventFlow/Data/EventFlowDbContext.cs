using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ZenCrm.EventFlow.Data;

[ConnectionStringName(EventFlowDbProperties.ConnectionStringName)]
public class EventFlowDbContext : AbpDbContext<EventFlowDbContext>, IEventFlowDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public EventFlowDbContext(DbContextOptions<EventFlowDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureEventFlow();
    }
}
