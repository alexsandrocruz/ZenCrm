using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace ZenCrm.Finance.Web.Menus;

public class FinanceMenuContributor : IMenuContributor
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
        context.Menu.AddItem(new ApplicationMenuItem(FinanceMenus.Prefix, displayName: "Finance", "~/Finance", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
