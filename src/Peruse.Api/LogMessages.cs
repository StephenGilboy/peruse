using System.Net;

namespace Peruse.Api;

public static partial class LogMessages
{
    [LoggerMessage(
        LogLevel.Information,
        EventId = 100,
        EventName = "ApplicationStarting",
        Message = "Application starting up.")]
    public static partial void ApplicationStarting(this ILogger logger);
    
    [LoggerMessage(
        LogLevel.Information,
        EventId = 101,
        EventName = "ApplicationStarted",
        Message = "Application started up.")]
    public static partial void ApplicationStarted(this ILogger logger);
    
    [LoggerMessage(
        LogLevel.Information,
        EventId = 102,
        EventName = "ApplicationStopping",
        Message = "Application is shutting down...")]
    public static partial void ApplicationStopping(this ILogger logger);
    
    [LoggerMessage(
        LogLevel.Information,
        EventId = 103,
        EventName = "ApplicationStopped",
        Message = "Application has stopped.")]
    public static partial void ApplicationStopped(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 104,
        EventName = "InitializingPlaywright",
        Message = "Initializing Playwright...")]
    public static partial void InitializingPlaywright(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 105,
        EventName = "InitializedPlaywright",
        Message = "Playwright initialized.")]
    public static partial void InitializedPlaywright(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 106,
        EventName = "StartingBrowser",
        Message = "Starting browser...")]
    public static partial void StartingBrowser(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 107,
        EventName = "StartedBrowser",
        Message = "Browser started.")]
    public static partial void StartedBrowser(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 108,
        EventName = "StoppingBrowser",
        Message = "Stopping browser...")]
    public static partial void StoppingBrowser(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 109,
        EventName = "StoppedBrowser",
        Message = "Browser stopped.")]
    public static partial void StoppedBrowser(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 110,
        EventName = "StartingSession",
        Message = "Starting new session {SessionId}...")]
    public static partial void StartingSession(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 111,
        EventName = "StartedSession",
        Message = "Started Session {SessionId}.")]
    public static partial void StartedSession(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 112,
        EventName = "StoppingSession",
        Message = "Stopping session {SessionId}...")]
    public static partial void StoppingSession(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 113,
        EventName = "StoppedSession",
        Message = "Stopped session {SessionId}.")]
    public static partial void StoppedSession(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 114,
        EventName = "StartSessionFailed",
        Message = "Failed to start session {SessionId}: {Exception}")]
    public static partial void StartSessionFailed(this ILogger logger, Guid sessionId, Exception exception);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 115,
        EventName = "SessionNotFound",
        Message = "Session {SessionId} not found.")]
    public static partial void SessionNotFound(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 116,
        EventName = "NavigatingToPage",
        Message = "Navigating to page {Uri} for session {SessionId}...")]
    public static partial void NavigatingToPage(this ILogger logger, Uri uri, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 117,
        EventName = "NavigatedToPage",
        Message = "Navigated to page {Uri} for session {SessionId}.")]
    public static partial void NavigatedToPage(this ILogger logger, Uri uri, Guid sessionId);
    
    [LoggerMessage(LogLevel.Error,
        EventId = 118,
        EventName = "NavigateToPageRequestFailed",
        Message = "Navigate to page {Uri} returned an unsuccessful http response for session {SessionId}: {StatusCode} {Content}")]
    public static partial void NavigateToPageRequestFailed(this ILogger logger, Uri uri, Guid sessionId, int statusCode, string content);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 119,
        EventName = "NavigateToPageFailed",
        Message = "Failed to navigate to page {Uri} for session {SessionId}: {Exception}")]
    public static partial void NavigateToPageFailed(this ILogger logger, Uri uri, Guid sessionId,  Exception exception);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 120,
        EventName = "CreatingPage",
        Message = "Creating new page for session {SessionId}...")]
    public static partial void CreatingPage(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 121,
        EventName = "CreatedPage",
        Message = "Created new page for session {SessionId}.")]
    public static partial void CreatedPage(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 122,
        EventName = "GettingPage",
        Message = "Getting browser page for session {SessionId}...")]
    public static partial void GettingPage(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 123,
        EventName = "GotPage",
        Message = "Got browser page for session {SessionId}.")]
    public static partial void GotPage(this ILogger logger, Guid sessionId);
    
    [LoggerMessage(LogLevel.Information,
        EventId = 124,
        EventName = "GetPageError",
        Message = "An exception was thrown when tyring to get browser page session {SessionId}. {Exception}")]
    public static partial void GetPageError(this ILogger logger, Guid sessionId, Exception exception);
    
    
}