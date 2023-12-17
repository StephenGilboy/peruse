using Peruse.Api.Requests;
using Refit;

namespace Peruse.Chat;

public record OpenBrowserResponse(Guid SessionId);
public record NavigateToPageResponse(Guid SessionId, string Content, int StatusCode, string StatusDescription);

public interface IPeruseApi
{
    [Post("/api/browser/open")]
    Task<IApiResponse<OpenBrowserResponse>> OpenBrowser();
    
    [Post("/api/browser/navigate")]
    Task<IApiResponse<NavigateToPageResponse>> NavigateToPage([Body(BodySerializationMethod.Serialized)]NavigateToPageRequest navigateToPageRequest);
}