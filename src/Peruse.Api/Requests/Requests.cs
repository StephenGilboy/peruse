using System.ComponentModel.DataAnnotations;

namespace Peruse.Api.Requests;

public class OpenBrowser
{
    public string? BrowserType { get; set; } = "chromium";
}

public class NavigateToPageRequest
{
    [Required]
    public Guid SessionId { get; set; }
    
    [Required, DataType(DataType.Url)]
    public Uri Url { get; set; } = null!;
}