namespace ZenCrm.Permissions;

public static class ZenCrmPermissions
{
    public const string GroupName = "ZenCrm";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // CRM Sales Permissions
    public static class SalesLeads
    {
        public const string Default = GroupName + ".SalesLeads";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Assign = Default + ".Assign";
        public const string Convert = Default + ".Convert";
        public const string ViewAll = Default + ".ViewAll";
        public const string ManageAll = Default + ".ManageAll";
    }

    public static class Clients
    {
        public const string Default = GroupName + ".Clients";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Assign = Default + ".Assign";
        public const string ViewAll = Default + ".ViewAll";
        public const string ManageAll = Default + ".ManageAll";
    }

    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Assign = Default + ".Assign";
        public const string SetAsPrimary = Default + ".SetAsPrimary";
        public const string SetAsDecisionMaker = Default + ".SetAsDecisionMaker";
        public const string ViewAll = Default + ".ViewAll";
        public const string ManageAll = Default + ".ManageAll";
    }

    public static class Interactions
    {
        public const string Default = GroupName + ".Interactions";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Complete = Default + ".Complete";
        public const string Cancel = Default + ".Cancel";
        public const string Postpone = Default + ".Postpone";
        public const string ViewAll = Default + ".ViewAll";
        public const string ManageAll = Default + ".ManageAll";
    }

    public static class SalesOpportunities
    {
        public const string Default = GroupName + ".SalesOpportunities";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string AdvanceStage = Default + ".AdvanceStage";
        public const string Close = Default + ".Close";
        public const string SetPriority = Default + ".SetPriority";
        public const string ViewAll = Default + ".ViewAll";
        public const string ManageAll = Default + ".ManageAll";
    }

    // Dashboard and Reports
    public static class Dashboard
    {
        public const string Default = GroupName + ".Dashboard";
        public const string View = Default + ".View";
        public const string ViewAllData = Default + ".ViewAllData";
        public const string Export = Default + ".Export";
    }

    // Management permissions
    public static class Management
    {
        public const string Default = GroupName + ".Management";
        public const string ViewAllLeads = Default + ".ViewAllLeads";
        public const string ViewAllClients = Default + ".ViewAllClients";
        public const string ViewAllCustomers = Default + ".ViewAllCustomers";
        public const string ViewAllInteractions = Default + ".ViewAllInteractions";
        public const string ViewAllOpportunities = Default + ".ViewAllOpportunities";
        public const string AssignAnyEntity = Default + ".AssignAnyEntity";
        public const string ModifyAnyEntity = Default + ".ModifyAnyEntity";
    }
}
