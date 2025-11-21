using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using ZenCrm.Communication.Services;

namespace ZenCrm.Communication.Jobs;

/// <summary>
/// Background job to process overdue queued messages
/// </summary>
public class ProcessOverdueMessagesJob : AsyncBackgroundJob<ProcessOverdueMessagesJobArgs>, ITransientDependency
{
    private readonly ICommunicationManager _communicationManager;
    private readonly ILogger<ProcessOverdueMessagesJob> _logger;

    public ProcessOverdueMessagesJob(
        ICommunicationManager communicationManager,
        ILogger<ProcessOverdueMessagesJob> logger)
    {
        _communicationManager = communicationManager;
        _logger = logger;
    }

    public override async Task ExecuteAsync(ProcessOverdueMessagesJobArgs args)
    {
        _logger.LogInformation("Starting ProcessOverdueMessagesJob");

        try
        {
            var overdueMessages = await _communicationManager.GetOverdueMessagesAsync();

            if (!overdueMessages.Any())
            {
                _logger.LogInformation("No overdue messages found");
                return;
            }

            _logger.LogInformation("Found {Count} overdue messages to process", overdueMessages.Count);

            foreach (var message in overdueMessages)
            {
                try
                {
                    _logger.LogDebug("Processing overdue message {MessageId}", message.Id);
                    await _communicationManager.ProcessMessageAsync(message.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process overdue message {MessageId}: {Error}",
                        message.Id, ex.Message);
                    // Continue with other messages
                }
            }

            _logger.LogInformation("ProcessOverdueMessagesJob completed. Processed {Count} messages",
                overdueMessages.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProcessOverdueMessagesJob failed: {Error}", ex.Message);
            throw;
        }
    }
}

/// <summary>
/// Arguments for ProcessOverdueMessagesJob
/// </summary>
public class ProcessOverdueMessagesJobArgs
{
    // No arguments needed for this job
}