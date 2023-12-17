using Microsoft.Playwright;

namespace Peruse.Api.Wttw.PlaywrightWindow;

public class Session
{
    public Guid SessionId { get; set; }
    public IBrowserContext BrowserContext { get; set; }
    public int CurrentPageIndex { get; set; }
    public DateTime LastUsed { get; set; }
    
    public bool IsExpired => DateTime.Now - LastUsed > TimeSpan.FromMinutes(15);
}