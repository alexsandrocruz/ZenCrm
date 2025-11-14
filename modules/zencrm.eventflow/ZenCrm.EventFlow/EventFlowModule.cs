using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using ZenCrm.EventFlow.Data;
using Volo.Abp.AspNetCore.Mvc;

namespace ZenCrm.EventFlow;

[DependsOn(
    typeof(EventFlowContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class EventFlowModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(EventFlowModule).Assembly);
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<EventFlowModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EventFlowModule>(validate: true);
        });
        
        context.Services.AddAbpDbContext<EventFlowDbContext>(options =>
        {
            options.AddDefaultRepositories<IEventFlowDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });
    }
}
