using CSharpFunctionalExtensions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Playwright;
using Peruse.Api.Wttw.Commands;
using Peruse.Api.Wttw.Responses;

namespace Peruse.Api.Wttw.PlaywrightWindow.CommandHandlers;

/// <summary>
/// Navigate to a page in the browser
/// </summary>
/// <param name="playwrightContext">Playwright Singleton Context</param>
/// <param name="logger">Logger</param>
public class NavigateToPageCommandHandler(PlaywrightWindowContext playwrightContext, ILogger<NavigateToPageCommandHandler> logger) : IRequestHandler<NavigateToPage, Result<NavigateToPageResponse, Exception>>
{
    /// <summary>
    /// Navigates to a page in the browser using the Playwright API
    /// </summary>
    /// <param name="request">NavigateToPageCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>NavigateToPageResponse if Success or Exception on failure</returns>
    public async Task<Result<NavigateToPageResponse, Exception>> Handle(NavigateToPage request,
        CancellationToken cancellationToken)
    {
        var (sessionId, uri) = request;
        logger.NavigatingToPage(uri, sessionId);
        var pageResult = await playwrightContext.GetPage(sessionId);
        if (!pageResult.IsSuccess)
            return Result.Failure<NavigateToPageResponse, Exception>(pageResult.Error);

        try
        {
            var page = pageResult.Value;
            var response = await page.GotoAsync(uri.ToString(), new PageGotoOptions()
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });
            if (response is null)
            {
                logger.NavigateToPageRequestFailed(uri, sessionId, 0, "Response object was null");
                return Result.Failure<NavigateToPageResponse, Exception>(new Exception($"Navigate to {uri} failed with no response"));
            }
            if (!response.Ok)
            {
                var body = await response.TextAsync();
                logger.NavigateToPageRequestFailed(uri, sessionId, response.Status, body);
                return Result.Success<NavigateToPageResponse, Exception>(
                    new NavigateToPageResponse(sessionId, body, response.Status, response.StatusText));
            }
            
            logger.NavigatedToPage(uri, sessionId);
            var content = await page.ContentAsync();
            
            var document = new HtmlDocument();
            document.LoadHtml(content);
            var sanitized = Utilities.SanitizeHtmlDocument(document);
            return Result.Success<NavigateToPageResponse, Exception>(
                new NavigateToPageResponse(sessionId, sanitized, response.Status, response.StatusText));
        }
        catch (Exception e)
        {
            logger.NavigateToPageFailed(uri, sessionId, e);
            return Result.Failure<NavigateToPageResponse, Exception>(e);
        }
    }
}