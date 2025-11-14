using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace ZenCrm.EventFlow.Web.Menus;

public class EventFlowMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        context.Menu.AddItem(new ApplicationMenuItem(EventFlowMenus.Prefix, displayName: "EventFlow", "~/EventFlow", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
