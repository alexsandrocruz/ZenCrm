namespace ZenCrm.Sales;

public enum PipelineStage
{
    /// <summary>
    /// Initial lead capture
    /// </summary>
    Lead = 1,

    /// <summary>
    /// Lead qualification in progress
    /// </summary>
    Qualifying = 2,

    /// <summary>
    /// Lead qualified, ready for engagement
    /// </summary>
    Qualified = 3,

    /// <summary>
    /// Requirements analysis completed
    /// </summary>
    Analysis = 4,

    /// <summary>
    /// Solution proposal being prepared
    /// </summary>
    Proposal = 5,

    /// <summary>
    /// Proposal sent to client
    /// </summary>
    ProposalSent = 6,

    /// <summary>
    /// Negotiation phase
    /// </summary>
    Negotiation = 7,

    /// <summary>
    /// Verbal agreement obtained
    /// </summary>
    VerbalCommitment = 8,

    /// <summary>
    /// Contract being prepared/signed
    /// </summary>
    Closing = 9,

    /// <summary>
    /// Deal won - converted to customer
    /// </summary>
    Won = 10,

    /// <summary>
    /// Deal lost
    /// </summary>
    Lost = 11,

    /// <summary>
    /// Deal put on hold
    /// </summary>
    OnHold = 12
}