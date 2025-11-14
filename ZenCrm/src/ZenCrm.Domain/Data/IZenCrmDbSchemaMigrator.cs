using System.Threading.Tasks;

namespace ZenCrm.Data;

public interface IZenCrmDbSchemaMigrator
{
    Task MigrateAsync();
}
