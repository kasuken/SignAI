using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using PostmarkDotNet.Webhooks;
using SignAI.Func.Services;
using SignAI.Func.Settings;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SignAI.Func;

public class Postmark
{
    private readonly ILogger<Postmark> _logger;
    private readonly IConfiguration _configuration;
    private readonly EmailSettings _emailSettings;
    private readonly PostmarkSettings _postmarkSettings;
    private readonly SignatureService _signatureService;

    public Postmark(
        ILogger<Postmark> logger,
        IConfiguration configuration,
        IOptions<EmailSettings> emailSettings,
        IOptions<PostmarkSettings> postmarkSettings,
        SignatureService signatureService)
    {
        _logger = logger;
        _configuration = configuration;
        _emailSettings = emailSettings.Value;
        _postmarkSettings = postmarkSettings.Value;
        _signatureService = signatureService;

        // Log configuration values for debugging
        LogConfigurationValues();
    }

    private void LogConfigurationValues()
    {
        _logger.LogInformation("Email Settings loaded: {EmailSettings}",
            System.Text.Json.JsonSerializer.Serialize(_emailSettings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
        _logger.LogInformation("Postmark Settings loaded: {PostmarkSettings}",
            System.Text.Json.JsonSerializer.Serialize(_postmarkSettings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        _logger.LogInformation("Configuration Email:DefaultSender: {DefaultSender}",
            _configuration["Email:DefaultSender"]);
        _logger.LogInformation("Direct IConfiguration value for PostmarkServerToken: {Token}",
            _configuration["PostmarkServerToken"]);
    }

    [Function("Postmark")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            _logger.LogDebug($"Received request body: {requestBody}");

            var webhook = JsonConvert.DeserializeObject<PostmarkInboundWebhookMessage>(requestBody);
            _logger.LogInformation($"Deserialized webhook: {webhook}");

            if (webhook == null)
            {
                _logger.LogError("Failed to deserialize webhook message.");
                return new BadRequestObjectResult("Invalid webhook message.");
            }

            // create the signature from the webhook data
            _logger.LogInformation("Extracting user information from webhook message");
            var signature = await _signatureService.GenerateSignature(webhook.TextBody);

            // create the html answer for the email, with a default message and the signature
            _logger.LogInformation("Creating HTML email body with signature");

            var htmlBody = _emailSettings.DefaultHtmlTemplate
                .Replace("{{signature}}", signature.SignatureHtml);

            // Process the webhook message
            _logger.LogInformation($"Processing webhook message with ID: {webhook.MessageID}");

            var to = webhook.From;
            _logger.LogInformation($"Sending email to: {to}");

            var from = _emailSettings.DefaultSender;
            _logger.LogInformation($"Using sender from configuration: {from}");

            var message = new PostmarkMessage
            {
                To = to,
                From = from,
                TrackOpens = _emailSettings.TrackOpens,
                Subject = _emailSettings.DefaultSubject,
                //TextBody = _emailSettings.DefaultTextBody,
                HtmlBody = htmlBody,
                Tag = _emailSettings.DefaultTag,
                MessageStream = _emailSettings.DefaultMessageStream
            };

            // Initialize the PostmarkClient with the server token
            var serverToken = _postmarkSettings.ServerToken;
            if (string.IsNullOrEmpty(serverToken))
            {
                // Fallback to the value in Values section if Postmark section not loaded correctly
                serverToken = _configuration["PostmarkServerToken"];
                _logger.LogWarning("Using fallback PostmarkServerToken from Values section: {Token}", serverToken);
            }

            var client = new PostmarkClient(serverToken);
            var sendResult = await client.SendMessageAsync(message);

            if (sendResult.Status == PostmarkStatus.Success)
            {
                _logger.LogInformation($"Email sent successfully with ID: {sendResult.MessageID}");
                return new OkObjectResult($"Email sent successfully with ID: {sendResult.MessageID}");
            }
            else
            {
                _logger.LogError($"Failed to send email: {sendResult.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the Postmark webhook");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}