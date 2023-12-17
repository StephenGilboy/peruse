namespace Peruse.Chat.ChatFeature;

/// <summary>
/// Request sent to the API to navigate to a webpage.
/// </summary>
/// <param name="SessionId">Browser SessionId</param>
/// <param name="Url">Url to visit</param>
public record NavigateToPageRequest(Guid SessionId, string Url);