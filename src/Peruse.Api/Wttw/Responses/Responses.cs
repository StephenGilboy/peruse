using System.Net;
using HtmlAgilityPack;

namespace Peruse.Api.Wttw.Responses;

public record StartSessionResponse(Guid SessionId);

public record NavigateToPageResponse(Guid SessionId, string Html, int StatusCode, string StatusDescription);