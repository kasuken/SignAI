namespace SignAI.Func.Settings;

public class AISettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string ModelId { get; set; } = "gpt-4";
    public int MaxTokens { get; set; } = 1000;
    public float Temperature { get; set; } = 0.7f;
    public string SignaturePrompt { get; set; } = @"You are a professional HTML designer who creates minimalist,
                                                   responsive, accessible email signatures in monochrome. Use table layouts and inline styles.
                                                   Always reply with HTML only. Use the user's info and preferences to customize the signature.
                                                   Avoid add images in the signature. You can use emojis and coloured text if neded.
                                                   User's Info:
                                                   ```
                                                    {{$userInfo}}
                                                   ```
                                                   ";
}