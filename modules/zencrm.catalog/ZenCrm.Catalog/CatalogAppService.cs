using ZenCrm.Catalog.Localization;
using Volo.Abp.Application.Services;

namespace ZenCrm.Catalog;

public abstract class CatalogAppService : ApplicationService
{
    protected CatalogAppService()
    {
        LocalizationResource = typeof(CatalogResource);
        ObjectMapperContext = typeof(CatalogModule);
    }
}
