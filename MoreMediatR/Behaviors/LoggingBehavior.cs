using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoreMediatR.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;
            var requestGuid = Guid.NewGuid().ToString();

            var requestNameWithGuid = $"{requestName} [{requestGuid}]";

            _logger.LogInformation($"[START] {requestNameWithGuid}");
            TResponse response;

            try
            {
                response = await next();
            }
            finally
            {
                _logger.LogInformation(
                    $"[END] {requestNameWithGuid}");
            }

            return response;
        }
    }
}
