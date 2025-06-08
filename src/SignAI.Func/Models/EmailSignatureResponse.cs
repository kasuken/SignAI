namespace SignAI.Func.Models;

public class EmailSignatureResponse
{
    public string Subject { get; set; } = string.Empty;
    public string SignatureHtml { get; set; } = string.Empty;
}