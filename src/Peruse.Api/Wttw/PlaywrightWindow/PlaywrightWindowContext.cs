using System.Collections.Concurrent;
using CSharpFunctionalExtensions;
using Microsoft.Playwright;

namespace Peruse.Api.Wttw.PlaywrightWindow;

public class SessionNotFoundException : Exception
{
    public SessionNotFoundException(Guid sessionId) : base($"Session {sessionId} not found")
    {
    }
}

public class PlaywrightWindowContext(ILogger<PlaywrightWindowContext> logger)
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private bool _isInitialized = false;
    private readonly ConcurrentDictionary<Guid, Session> _sessions = new();
    
    public async Task Initialize()
    {
        logger.InitializingPlaywright();
        _playwright = await Playwright.CreateAsync();
        logger.InitializedPlaywright();
        logger.StartingBrowser();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        logger.StartedBrowser();
        _isInitialized = true;        
    }
    
    public async Task<Result<Guid, Exception>> StartSession()
    {
        if (!_isInitialized)
            throw new InvalidProgramException("PlaywrightWindowContext not initialized");
        var sessionId = Guid.NewGuid();
        try
        {
            logger.StartingSession(sessionId);
            var session = new Session
            {
                SessionId = sessionId,
                BrowserContext = await _browser.NewContextAsync(),
                CurrentPageIndex = 0,
                LastUsed = DateTime.Now
            };
            _sessions.TryAdd(sessionId, session);
            logger.StartedSession(sessionId);
            return Result.Success<Guid, Exception>(sessionId);
        }
        catch (Exception e)
        {
            logger.StartSessionFailed(sessionId, e);
            return Result.Failure<Guid, Exception>(e);
        }
    }

    public async Task<Result<IPage, Exception>> GetPage(Guid sessionId)
    {
        if (!_isInitialized)
            throw new InvalidProgramException("PlaywrightWindowContext not initialized");
        try
        {
            if (!_sessions.TryGetValue(sessionId, out var session))
            {
                logger.SessionNotFound(sessionId);
                return Result.Failure<IPage, Exception>(new SessionNotFoundException(sessionId));
            }
            session.LastUsed = DateTime.Now;
            IPage page;
            if (session.BrowserContext.Pages.Count == 0)
            {
                logger.CreatingPage(sessionId);
                page = await session.BrowserContext.NewPageAsync();
                logger.CreatedPage(sessionId);
                session.CurrentPageIndex = session.BrowserContext.Pages.Count - 1;
            }
            else
            {
                logger.GettingPage(sessionId);
                page = session.BrowserContext.Pages[session.CurrentPageIndex];
                logger.GotPage(sessionId);
            }
            return Result.Success<IPage, Exception>(page);
        }
        catch (Exception e)
        {
            logger.GetPageError(sessionId, e);
            return Result.Failure<IPage, Exception>(e);
        }
    }
}