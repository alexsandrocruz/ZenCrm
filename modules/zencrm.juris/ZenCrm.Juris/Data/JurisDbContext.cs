using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ZenCrm.Juris.Data;

[ConnectionStringName(JurisDbProperties.ConnectionStringName)]
public class JurisDbContext : AbpDbContext<JurisDbContext>, IJurisDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public JurisDbContext(DbContextOptions<JurisDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureJuris();
    }
}
