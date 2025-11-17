namespace ZenCrm.Sales;

public enum LeadSource
{
    /// <summary>
    /// Lead from website form
    /// </summary>
    Website = 1,

    /// <summary>
    /// Lead from referral
    /// </summary>
    Referral = 2,

    /// <summary>
    /// Lead from cold calling
    /// </summary>
    ColdCall = 3,

    /// <summary>
    /// Lead from marketing campaign
    /// </summary>
    Campaign = 4,

    /// <summary>
    /// Lead from social media
    /// </summary>
    SocialMedia = 5,

    /// <summary>
    /// Lead from trade show/event
    /// </summary>
    Event = 6,

    /// <summary>
    /// Lead from online advertising
    /// </summary>
    OnlineAd = 7,

    /// <summary>
    /// Lead from email marketing
    /// </summary>
    Email = 8,

    /// <summary>
    /// Lead from partner
    /// </summary>
    Partner = 9,

    /// <summary>
    /// Lead source not specified
    /// </summary>
    Other = 10
}