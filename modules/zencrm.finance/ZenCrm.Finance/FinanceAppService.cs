using ZenCrm.Finance.Localization;
using Volo.Abp.Application.Services;

namespace ZenCrm.Finance;

public abstract class FinanceAppService : ApplicationService
{
    protected FinanceAppService()
    {
        LocalizationResource = typeof(FinanceResource);
        ObjectMapperContext = typeof(FinanceModule);
    }
}
