using ZenCrm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ZenCrm.Permissions;

public class ZenCrmPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ZenCrmPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(ZenCrmPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(ZenCrmPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(ZenCrmPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(ZenCrmPermissions.Books.Delete, L("Permission:Books.Delete"));

        // CRM Sales Permissions
        var salesLeadsPermission = myGroup.AddPermission(ZenCrmPermissions.SalesLeads.Default, L("Permission:SalesLeads"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.Create, L("Permission:SalesLeads.Create"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.Edit, L("Permission:SalesLeads.Edit"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.Delete, L("Permission:SalesLeads.Delete"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.Assign, L("Permission:SalesLeads.Assign"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.Convert, L("Permission:SalesLeads.Convert"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.ViewAll, L("Permission:SalesLeads.ViewAll"));
        salesLeadsPermission.AddChild(ZenCrmPermissions.SalesLeads.ManageAll, L("Permission:SalesLeads.ManageAll"));

        var clientsPermission = myGroup.AddPermission(ZenCrmPermissions.Clients.Default, L("Permission:Clients"));
        clientsPermission.AddChild(ZenCrmPermissions.Clients.Create, L("Permission:Clients.Create"));
        clientsPermission.AddChild(ZenCrmPermissions.Clients.Edit, L("Permission:Clients.Edit"));
        clientsPermission.AddChild(ZenCrmPermissions.Clients.Delete, L("Permission:Clients.Delete"));
        clientsPermission.AddChild(ZenCrmPermissions.Clients.Assign, L("Permission:Clients.Assign"));
        clientsPermission.AddChild(ZenCrmPermissions.Clients.ViewAll, L("Permission:Clients.ViewAll"));
        clientsPermission.AddChild(ZenCrmPermissions.Clients.ManageAll, L("Permission:Clients.ManageAll"));

        var customersPermission = myGroup.AddPermission(ZenCrmPermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.Delete, L("Permission:Customers.Delete"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.Assign, L("Permission:Customers.Assign"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.SetAsPrimary, L("Permission:Customers.SetAsPrimary"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.SetAsDecisionMaker, L("Permission:Customers.SetAsDecisionMaker"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.ViewAll, L("Permission:Customers.ViewAll"));
        customersPermission.AddChild(ZenCrmPermissions.Customers.ManageAll, L("Permission:Customers.ManageAll"));

        var interactionsPermission = myGroup.AddPermission(ZenCrmPermissions.Interactions.Default, L("Permission:Interactions"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.Create, L("Permission:Interactions.Create"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.Edit, L("Permission:Interactions.Edit"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.Delete, L("Permission:Interactions.Delete"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.Complete, L("Permission:Interactions.Complete"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.Cancel, L("Permission:Interactions.Cancel"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.Postpone, L("Permission:Interactions.Postpone"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.ViewAll, L("Permission:Interactions.ViewAll"));
        interactionsPermission.AddChild(ZenCrmPermissions.Interactions.ManageAll, L("Permission:Interactions.ManageAll"));

        var salesOpportunitiesPermission = myGroup.AddPermission(ZenCrmPermissions.SalesOpportunities.Default, L("Permission:SalesOpportunities"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.Create, L("Permission:SalesOpportunities.Create"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.Edit, L("Permission:SalesOpportunities.Edit"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.Delete, L("Permission:SalesOpportunities.Delete"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.AdvanceStage, L("Permission:SalesOpportunities.AdvanceStage"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.Close, L("Permission:SalesOpportunities.Close"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.SetPriority, L("Permission:SalesOpportunities.SetPriority"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.ViewAll, L("Permission:SalesOpportunities.ViewAll"));
        salesOpportunitiesPermission.AddChild(ZenCrmPermissions.SalesOpportunities.ManageAll, L("Permission:SalesOpportunities.ManageAll"));

        // Dashboard permissions
        var dashboardPermission = myGroup.AddPermission(ZenCrmPermissions.Dashboard.Default, L("Permission:Dashboard"));
        dashboardPermission.AddChild(ZenCrmPermissions.Dashboard.View, L("Permission:Dashboard.View"));
        dashboardPermission.AddChild(ZenCrmPermissions.Dashboard.ViewAllData, L("Permission:Dashboard.ViewAllData"));
        dashboardPermission.AddChild(ZenCrmPermissions.Dashboard.Export, L("Permission:Dashboard.Export"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ZenCrmPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ZenCrmResource>(name);
    }
}
