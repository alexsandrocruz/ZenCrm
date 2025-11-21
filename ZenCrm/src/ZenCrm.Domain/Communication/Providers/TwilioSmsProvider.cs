using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using ZenCrm.Communication.Entities;
using ZenCrm.Communication.Services;

namespace ZenCrm.Communication.Providers;

/// <summary>
/// Twilio SMS provider implementation
/// </summary>
public class TwilioSmsProvider : ISmsService
{
    private readonly ILogger<TwilioSmsProvider> _logger;
    private readonly TwilioConfig _config;

    public TwilioSmsProvider(
        ILogger<TwilioSmsProvider> logger,
        TwilioConfig config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task<MessageDeliveryResult> SendAsync(Message message)
    {
        try
        {
            _logger.LogInformation("Sending SMS to {PhoneNumber} with {Segments} segments",
                message.ToAddress, CalculateSegments(message.Content));

            // Format phone number
            var formattedNumber = await FormatPhoneNumberAsync(message.ToAddress, message.ToAddress);

            // Initialize Twilio client
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);

            // Create SMS message
            var smsMessage = await MessageResource.CreateAsync(
                body: message.Content,
                from: _config.FromPhoneNumber,
                to: new List<string> { formattedNumber }
            );

            _logger.LogInformation("SMS sent successfully to {PhoneNumber}. Message SID: {MessageSid}",
                message.ToAddress, smsMessage.Sid);

            return new MessageDeliveryResult
            {
                Success = true,
                ExternalMessageId = smsMessage.Sid,
                ProviderResponse = "SMS sent successfully via Twilio",
                Price = await GetMessagePrice(smsMessage.Sid)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS to {PhoneNumber}: {Error}",
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

    public async Task<BatchMessageResult> SendBatchAsync(Message message, string[] recipients)
    {
        var result = new BatchMessageResult();
        result.TotalRecipients = recipients.Length;

        // Format all phone numbers first
        var formattedNumbers = new List<string>();
        foreach (var recipient in recipients)
        {
            try
            {
                var formatted = await FormatPhoneNumberAsync(recipient);
                formattedNumbers.Add(formatted);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to format phone number {PhoneNumber}", recipient);
                result.Results.Add(new SmsSendResult
                {
                    PhoneNumber = recipient,
                    Success = false,
                    ErrorMessage = $"Invalid phone number: {ex.Message}"
                });
            }
        }

        // Send SMS to each formatted number
        var tasks = formattedNumbers.Select(async (phoneNumber, index) =>
        {
            try
            {
                TwilioClient.Init(_config.AccountSid, _config.AuthToken);

                var smsMessage = await MessageResource.CreateAsync(
                    body: message.Content,
                    from: _config.FromPhoneNumber,
                    to: new List<string> { phoneNumber }
                );

                return new SmsSendResult
                {
                    PhoneNumber = recipients[index],
                    Success = true,
                    MessageId = smsMessage.Sid,
                    SentAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SMS to {PhoneNumber}", phoneNumber);
                return new SmsSendResult
                {
                    PhoneNumber = recipients[index],
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        });

        var sendResults = await Task.WhenAll(tasks);

        foreach (var sendResult in sendResults)
        {
            result.Results.Add(sendResult);
            if (sendResult.Success)
            {
                result.SuccessfulSends++;
            }
            else
            {
                result.FailedSends++;
            }
        }

        return result;
    }

    public async Task<bool> ValidatePhoneNumberAsync(string phoneNumber)
    {
        try
        {
            // Remove any non-digit characters except + at the beginning
            var cleaned = System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"[^\d+]", "");

            // Check basic format
            if (string.IsNullOrWhiteSpace(cleaned))
                return false;

            // Check if it's a valid international number (starts with + and has 7-15 digits)
            if (cleaned.StartsWith("+"))
            {
                return cleaned.Length >= 8 && cleaned.Length <= 16;
            }

            // For non-international, check if it's valid (10-15 digits)
            return cleaned.Length >= 10 && cleaned.Length <= 15;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating phone number {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<PhoneValidationResult> ValidatePhoneNumbersAsync(string[] phoneNumbers)
    {
        var result = new PhoneValidationResult();

        foreach (var phoneNumber in phoneNumbers)
        {
            try
            {
                var isValid = await ValidatePhoneNumberAsync(phoneNumber);

                if (isValid)
                {
                    var formatted = await FormatPhoneNumberAsync(phoneNumber);
                    result.ValidNumbers.Add(new ValidPhoneNumber
                    {
                        Original = phoneNumber,
                        Formatted = formatted,
                        CountryCode = ExtractCountryCode(formatted),
                        Type = DeterminePhoneType(formatted)
                    });
                }
                else
                {
                    result.InvalidNumbers.Add(new InvalidPhoneNumber
                    {
                        Original = phoneNumber,
                        ErrorReason = "Invalid phone number format"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error validating phone number {PhoneNumber}", phoneNumber);
                result.InvalidNumbers.Add(new InvalidPhoneNumber
                {
                    Original = phoneNumber,
                    ErrorReason = $"Validation error: {ex.Message}"
                });
            }
        }

        return result;
    }

    public async Task<string> FormatPhoneNumberAsync(string phoneNumber, string? countryCode = null)
    {
        try
        {
            // Use Twilio's built-in lookup service if available
            // For now, implement basic formatting

            // Remove all non-digit characters
            var digits = System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"\D", "");

            // If it starts with + and looks like a full international number, return as is
            if (phoneNumber.StartsWith("+") && digits.Length >= 8)
            {
                return $"+{digits}";
            }

            // If no country code provided, assume the number is in the default country
            // For now, assume US if no country code provided
            var defaultCountryCode = countryCode ?? "1";

            // If number has 10 digits, assume it's a national number
            if (digits.Length == 10)
            {
                return $"+{defaultCountryCode}{digits}";
            }

            // If number has fewer than 10 digits, it's invalid
            if (digits.Length < 10)
            {
                throw new ArgumentException($"Invalid phone number: {phoneNumber}");
            }

            // For 11+ digits, if it starts with 0, remove the 0 and add country code
            if (digits.StartsWith("0"))
            {
                return $"+{defaultCountryCode}{digits.Substring(1)}";
            }

            // For other cases, assume it's already properly formatted
            return $"+{digits}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error formatting phone number {PhoneNumber}", phoneNumber);
            throw new ArgumentException($"Failed to format phone number: {phoneNumber}", ex);
        }
    }

    public async Task<SmsDeliveryStatus> GetDeliveryStatusAsync(string messageId)
    {
        try
        {
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);

            var message = await MessageResource.FetchAsync(messageId);

            return new SmsDeliveryStatus
            {
                MessageId = message.Sid,
                Status = MapTwilioStatusToSmsStatus(message.Status),
                SentAt = message.DateSent?.DateTime,
                DeliveredAt = message.DateUpdated?.DateTime,
                ErrorCode = message.ErrorCode?.ToString(),
                ErrorMessage = message.ErrorMessage,
                Price = message.Price ?? 0,
                RetryCount = 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SMS status for message {MessageId}", messageId);
            throw new InvalidOperationException($"Failed to get SMS status: {messageId}", ex);
        }
    }

    public async Task<SmsMessageDetails> GetMessageDetailsAsync(string messageId)
    {
        try
        {
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);

            var message = await MessageResource.FetchAsync(messageId);

            return new SmsMessageDetails
            {
                MessageId = message.Sid,
                From = message.From.ToString(),
                To = message.To.ToString(),
                Body = message.Body,
                Status = MapTwilioStatusToSmsStatus(message.Status),
                CreatedAt = message.DateCreated?.DateTime,
                SentAt = message.DateSent?.DateTime,
                DeliveredAt = message.DateUpdated?.DateTime,
                Segments = 1, // Twilio handles this internally
                Price = message.Price ?? 0,
                Currency = message.PriceUnit ?? "USD"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SMS details for message {MessageId}", messageId);
            throw new InvalidOperationException($"Failed to get SMS details: {messageId}", ex);
        }
    }

    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            // Check if Twilio configuration is valid
            if (string.IsNullOrWhiteSpace(_config.AccountSid) ||
                string.IsNullOrWhiteSpace(_config.AuthToken) ||
                string.IsNullOrWhiteSpace(_config.FromPhoneNumber))
            {
                return false;
            }

            // Try to validate account credentials
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);
            var account = await AccountResource.FetchAsync();

            return account.Status == AccountStatus.Active;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Twilio service is not available: {Error}", ex.Message);
            return false;
        }
    }

    public async Task<SmsCostResult> CalculateCostAsync(string phoneNumber, int messageLength, string? countryCode = null)
    {
        try
        {
            // Format the phone number to determine country
            var formattedNumber = await FormatPhoneNumberAsync(phoneNumber, countryCode);
            var destinationCountry = ExtractCountryCode(formattedNumber);

            // Twilio pricing varies by destination country
            // For estimation purposes, use average rates
            var pricePerSegment = GetPricePerSegment(destinationCountry);
            var segments = CalculateSegments(new string(' ', messageLength));

            return new SmsCostResult
            {
                Price = pricePerSegment * segments,
                Currency = "USD",
                Segments = segments,
                CountryCode = destinationCountry,
                IsInternational = destinationCountry != _config.DefaultCountryCode,
                PricePerSegment = pricePerSegment.ToString("F4")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating SMS cost for {PhoneNumber}", phoneNumber);
            return new SmsResult();
        }
    }

    private static int CalculateSegments(string message)
    {
        // GSM characters are 7-bit, but for simplicity, use 160 character segments
        const int segmentSize = 160;

        if (string.IsNullOrEmpty(message))
            return 0;

        // For messages longer than segment size, they use multi-part SMS
        return (int)Math.Ceiling((double)message.Length / segmentSize);
    }

    private static string ExtractCountryCode(string formattedPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(formattedPhoneNumber) || !formattedPhoneNumber.StartsWith("+"))
            return "1"; // Default to US

        // Extract country code from + followed by 1-3 digits
        var match = System.Text.RegularExpressions.Regex.Match(formattedPhoneNumber, @"^\+(\d{1,3})");
        return match.Success ? match.Groups[1].Value : "1";
    }

    private static string DeterminePhoneType(string formattedPhoneNumber)
    {
        // Very basic determination - in a real implementation, you'd use a phone number library
        var countryCode = ExtractCountryCode(formattedPhoneNumber);

        // Mobile number ranges are country-specific
        // This is a simplified determination
        return countryCode switch
        {
            "1" => "mobile", // Assume mobile for US numbers
            "55" => "mobile", // Assume mobile for Brazil numbers
            "44" => "mobile", // Assume mobile for UK numbers
            _ => "landline"
        };
    }

    private static SmsStatus MapTwilioStatusToSmsStatus(MessageResource.StatusEnum twilioStatus)
    {
        return twilioStatus.ToString().ToLowerInvariant() switch
        {
            "accepted" => SmsStatus.Queued,
            "queued" => SmsStatus.Queued,
            "sending" => SmsStatus.Sending,
            "sent" => SmsStatus.Sent,
            "delivered" => SmsStatus.Delivered,
            "read" => SmsStatus.Read,
            "failed" => SmsStatus.Failed,
            "undelivered" => SmsStatus.Undelivered,
            "rejected" => SmsStatus.Failed, // Map Rejected to Failed
            "canceled" => SmsStatus.Canceled,
            _ => SmsStatus.Failed
        };
    }

    private async Task<double> GetMessagePrice(string messageId)
    {
        try
        {
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);
            var message = await MessageResource.FetchAsync(messageId);
            return message.Price ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not get price for SMS message {MessageId}", messageId);
            return 0;
        }
    }

    private double GetPricePerSegment(string countryCode)
    {
        // Simplified pricing model
        // In reality, Twilio has different pricing per destination
        return countryCode switch
        {
            "1" => 0.0079,    // United States
            "55" => 0.0085,   // Brazil
            "44" => 0.0081,   // United Kingdom
            "1" => 0.0079,    // Canada (same as US)
            "52" => 0.0072,   // Mexico
            "34" => 0.0081,   // Spain
            "49" => 0.0081,   // Germany
            "33" => 0081,   // France
            "39" => 0.0081,    // Italy
            _ => 0.0099     // Default for other countries
        };
    }

    private static bool IsRetryableError(Exception exception)
    {
        var errorMessage = exception.Message.ToLowerInvariant();

        // Network-related errors that might be temporary
        if (errorMessage.Contains("timeout") ||
            errorMessage.Contains("connection") ||
            errorMessage.Contains("network") ||
            errorMessage.Contains("temporary"))
        {
            return true;
        }

        // Twilio service temporary unavailable
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

/// <summary>
/// Twilio configuration
/// </summary>
public class TwilioConfig
{
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromPhoneNumber { get; set; } = string.Empty;
    public string DefaultCountryCode { get; set; } = "1";
}

/// <summary>
/// Default SMS implementation when no provider is configured
/// </summary>
public class SmsResult
{
    public double Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int Segments { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public bool IsInternational { get; set; }
    public string PricePerSegment { get; set; } = string.Empty;
}