using System.Text.Json;
using Azure.AI.OpenAI;

namespace Peruse.Chat.ChatFeature;

/// <summary>
/// Static definitions of the functions that the assistant can use to interact with the browser.
/// </summary>
public static class ChatFunctions
{
    public static readonly ChatCompletionsFunctionToolDefinition OpenBrowser = new()
    {
        Name = "open_browser",
        Description = "Opens a new browser session",
        Parameters = BinaryData.FromObjectAsJson(new
        {
            Type = "object",
            Properties = new
            {
                BrowserType = new
                {
                    Type = "string",
                    Description = "The type of browser to open",
                    Default = "chromium",
                    Enum = new[] { "chromium", "firefox", "webkit" }
                }
            }
        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
    };
    
    public static readonly ChatCompletionsFunctionToolDefinition NavigateToPage = new()
    {
        Name = "navigate_to_page",
        Description = "Navigates to a page",
        Parameters = BinaryData.FromObjectAsJson(new
        {
            Type = "object",
            Properties = new
            {
                SessionId = new
                {
                    Type = "string",
                    Description = "The session id to returned from open_browser",
                    Format = "uuid"
                },
                Url = new
                {
                    Type = "string",
                    Description = "The url to navigate to",
                    Format = "uri"
                }
            }
        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
    };
}