using CSharpFunctionalExtensions;
using MediatR;
using Peruse.Api.Wttw.Responses;

namespace Peruse.Api.Wttw.Commands;

public record StartSession : IRequest<Result<StartSessionResponse, Exception>>;
public record NavigateToPage(Guid SessionId, Uri Uri) : IRequest<Result<NavigateToPageResponse, Exception>>;
