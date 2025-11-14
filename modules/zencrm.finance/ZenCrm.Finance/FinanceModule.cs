using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using ZenCrm.Finance.Data;
using Volo.Abp.AspNetCore.Mvc;

namespace ZenCrm.Finance;

[DependsOn(
    typeof(FinanceContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FinanceModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FinanceModule).Assembly);
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FinanceModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FinanceModule>(validate: true);
        });
        
        context.Services.AddAbpDbContext<FinanceDbContext>(options =>
        {
            options.AddDefaultRepositories<IFinanceDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}
