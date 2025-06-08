namespace SignAI.Func.Settings;

public class EmailSettings
{
    public string DefaultSender { get; set; }
    public string DefaultSubject { get; set; } = "Your New Awesome Signature";
    public bool TrackOpens { get; set; } = true;
    public string DefaultTextBody { get; set; } = "Plain Text Body";
    public string DefaultHtmlTemplate { get; set; } = "<html><body>This is your new email signature! <br /> <hr /> <br /> {{signature}} </body></html>";
    public string DefaultTag { get; set; } = "Your New Awesome Signature";
    public string DefaultMessageStream { get; set; } = "outbound";
    public bool EnableReplyTracking { get; set; } = true;
}