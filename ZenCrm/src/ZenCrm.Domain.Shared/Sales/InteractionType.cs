namespace ZenCrm.Sales;

public enum InteractionType
{
    /// <summary>
    /// Phone call interaction
    /// </summary>
    PhoneCall = 1,

    /// <summary>
    /// Email interaction
    /// </summary>
    Email = 2,

    /// <summary>
    /// SMS text message
    /// </summary>
    SMS = 3,

    /// <summary>
    /// WhatsApp message
    /// </summary>
    WhatsApp = 4,

    /// <summary>
    /// Face-to-face meeting
    /// </summary>
    Meeting = 5,

    /// <summary>
    /// Virtual meeting (Zoom, Teams, etc.)
    /// </summary>
    VirtualMeeting = 6,

    /// <summary>
    /// In-person office visit
    /// </summary>
    OfficeVisit = 7,

    /// <summary>
    /// Task or follow-up activity
    /// </summary>
    Task = 8,

    /// <summary>
    /// Note or comment
    /// </summary>
    Note = 9,

    /// <summary>
    /// Social media interaction
    /// </summary>
    SocialMedia = 10,

    /// <summary>
    /// Video conference
    /// </summary>
    VideoConference = 11,

    /// <summary>
    /// Web conference
    /// </summary>
    WebConference = 12,

    /// <summary>
    /// Other type of interaction
    /// </summary>
    Other = 99
}