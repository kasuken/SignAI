using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Newtonsoft.Json;
using SignAI.Func.Models;
using SignAI.Func.Settings;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SignAI.Func.Services;

public class SignatureService
{
    private readonly ILogger<SignatureService> _logger;
    private readonly Kernel _kernel;
    private readonly AISettings _aiSettings;

    public SignatureService(
        ILogger<SignatureService> logger,
        Kernel kernel,
        IOptions<AISettings> aiSettings)
    {
        _logger = logger;
        _kernel = kernel;
        _aiSettings = aiSettings.Value;
    }

    public async Task<EmailSignatureResponse> GenerateSignature(string userInfo)
    {
        try
        {
            _logger.LogInformation($"Generating signature with user info: {userInfo}");

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0.7,
                #pragma warning disable SKEXP0010
                ResponseFormat = typeof(EmailSignatureResponse)
            };

            var signatureFunction = _kernel.CreateFunctionFromPrompt(_aiSettings.SignaturePrompt, executionSettings);

            var arguments = new KernelArguments
            {
                { "userInfo", userInfo }
            };

            _logger.LogInformation("Sending request to AI model");
            var response = await _kernel.InvokeAsync(signatureFunction, arguments);
            var jsonResponse = response.GetValue<string>();

            _logger.LogInformation("AI model response received");

            var emailSignatureResponse = JsonConvert.DeserializeObject<EmailSignatureResponse>(jsonResponse);

            _logger.LogInformation($"HTML Signature: {emailSignatureResponse.SignatureHtml}");

            return emailSignatureResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating email signature");
            return new EmailSignatureResponse
            {
                Subject = "Your Email Signature",
                SignatureHtml = "<p>Failed to generate signature due to an error.</p>"
            };
        }
    }
}