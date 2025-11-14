using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace ZenCrm.Juris.Web.Menus;

public class JurisMenuContributor : IMenuContributor
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
        context.Menu.AddItem(new ApplicationMenuItem(JurisMenus.Prefix, displayName: "Juris", "~/Juris", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
