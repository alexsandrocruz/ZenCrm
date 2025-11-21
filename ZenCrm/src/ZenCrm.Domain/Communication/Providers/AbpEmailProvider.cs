using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Emailing;
using ZenCrm.Communication.Entities;
using ZenCrm.Communication.Services;

namespace ZenCrm.Communication.Providers;

/// <summary>
/// Email provider using ABP's built-in email service
/// </summary>
public class AbpEmailProvider : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AbpEmailProvider> _logger;

    public AbpEmailProvider(
        IEmailSender emailSender,
        ILogger<AbpEmailProvider> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<MessageDeliveryResult> SendAsync(Message message)
    {
        try
        {
            _logger.LogInformation("Sending email to {ToAddress} with subject {Subject}",
                message.ToAddress, message.Subject);

            // Create email body with proper HTML formatting
            var emailBody = FormatEmailBody(message.Content);

            // Send the email using ABP's simple API
            await _emailSender.SendAsync(
                message.ToAddress,
                message.Subject,
                emailBody,
                isBodyHtml: true
            );

            _logger.LogInformation("Email sent successfully to {ToAddress}", message.ToAddress);

            return new MessageDeliveryResult
            {
                Success = true,
                ProviderResponse = "Email sent successfully via ABP EmailSender"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToAddress}: {Error}",
                message.ToAddress, ex.Message);

            return new MessageDeliveryResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                ShouldRetry = IsRetryableError(ex),
                NextRetryDate = ShouldRetry(ex) ? DateTime.UtcNow.AddMinutes(5) : null
            };
        }
    }

    public async Task<bool> ValidateEmailAddressAsync(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var trimmedEmail = email.Trim();

            // Basic email validation
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(trimmedEmail);
                if (mailAddress.Address != trimmedEmail)
                    return false;
            }
            catch
            {
                return false;
            }

            // Additional validation rules can be added here
            // For example, checking against domain blacklist, etc.

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error validating email address {Email}: {Error}",
                email, ex.Message);
            return false;
        }
    }

    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            // Check if email sender is configured
            // This is a basic check - you might want to add more sophisticated validation
            return _emailSender != null && await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Email service is not available: {Error}", ex.Message);
            return false;
        }
    }

    private static string FormatEmailBody(string content)
    {
        // Basic HTML formatting for email body
        if (content.Contains("<html>") || content.Contains("<body>"))
        {
            // Content is already HTML formatted
            return content;
        }

        // Convert plain text to basic HTML
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Email Message</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            border-bottom: 2px solid #007acc;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }}
        .footer {{
            border-top: 1px solid #ddd;
            padding-top: 10px;
            margin-top: 20px;
            font-size: 12px;
            color: #666;
        }}
    </style>
</head>
<body>
    <div class=""header"">
        <h3>ZenCrm Communication</h3>
    </div>
    <div class=""content"">
        {System.Web.HttpUtility.HtmlEncode(content).Replace("\n", "<br />")}
    </div>
    <div class=""footer"">
        <p>Este email foi enviado automaticamente pelo ZenCrm.</p>
        <p>Se você não esperava receber esta mensagem, por favor ignore.</p>
    </div>
</body>
</html>";
    }

    private static bool IsRetryableError(Exception exception)
    {
        if (exception == null)
            return false;

        var errorMessage = exception.Message.ToLowerInvariant();

        // Network-related errors that might be temporary
        if (errorMessage.Contains("timeout") ||
            errorMessage.Contains("connection") ||
            errorMessage.Contains("network") ||
            errorMessage.Contains("temporary"))
        {
            return true;
        }

        // SMTP service temporarily unavailable
        if (errorMessage.Contains("service unavailable") ||
            errorMessage.Contains("try again later"))
        {
            return true;
        }

        return false;
    }

    private static bool ShouldRetry(Exception exception)
    {
        return IsRetryableError(exception);
    }
}