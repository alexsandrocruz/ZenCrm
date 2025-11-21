using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using ZenCrm.Communication.Services;

namespace ZenCrm.Communication.Jobs;

/// <summary>
/// Background job for sending messages
/// </summary>
public class SendMessageJob : AsyncBackgroundJob<SendMessageJobArgs>, ITransientDependency
{
    private readonly ICommunicationManager _communicationManager;
    private readonly ILogger<SendMessageJob> _logger;

    public SendMessageJob(
        ICommunicationManager communicationManager,
        ILogger<SendMessageJob> logger)
    {
        _communicationManager = communicationManager;
        _logger = logger;
    }

    public override async Task ExecuteAsync(SendMessageJobArgs args)
    {
        _logger.LogInformation("Processing SendMessageJob for message {MessageId}", args.MessageId);

        try
        {
            await _communicationManager.ProcessMessageAsync(args.MessageId);
            _logger.LogInformation("SendMessageJob completed successfully for message {MessageId}", args.MessageId);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "SendMessageJob failed for message {MessageId}: {Error}",
                args.MessageId, ex.Message);
            throw; // Re-throw to let ABP handle retry logic
        }
    }
}

/// <summary>
/// Arguments for SendMessageJob
/// </summary>
public class SendMessageJobArgs
{
    public Guid MessageId { get; set; }
}