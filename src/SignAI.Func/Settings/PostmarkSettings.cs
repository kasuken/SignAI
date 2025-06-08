namespace SignAI.Func.Settings;

public class PostmarkSettings
{
    public string ServerToken { get; set; } = string.Empty;
    public string AccountToken { get; set; } = string.Empty;
    public int RetryCount { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(2);
    public int TimeoutSeconds { get; set; } = 30;
    public bool UseHttps { get; set; } = true;
    public string BaseUrl { get; set; } = "https://api.postmarkapp.com";
    public string WebhookUrl { get; set; } = string.Empty;
}