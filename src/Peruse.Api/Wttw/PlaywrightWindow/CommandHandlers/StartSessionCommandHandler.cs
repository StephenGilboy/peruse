using CSharpFunctionalExtensions;
using MediatR;
using Peruse.Api.Wttw.Commands;
using Peruse.Api.Wttw.Responses;

namespace Peruse.Api.Wttw.PlaywrightWindow.CommandHandlers;

public class StartSessionCommandHandler(PlaywrightWindowContext playwrightContext, ILogger<StartSessionCommandHandler> logger) : IRequestHandler<StartSession, Result<StartSessionResponse, Exception>>
{
    public async Task<Result<StartSessionResponse, Exception>> Handle(StartSession request,
        CancellationToken cancellationToken)
    {
        var result = await playwrightContext.StartSession();
        return result.IsSuccess
            ? Result.Success<StartSessionResponse, Exception>(new StartSessionResponse(result.Value))
            : Result.Failure<StartSessionResponse, Exception>(result.Error);
    }
}