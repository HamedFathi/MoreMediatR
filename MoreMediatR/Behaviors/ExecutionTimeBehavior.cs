using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MoreMediatR.Behaviors
{
    public class ExecutionTimeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public ExecutionTimeBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;
            TResponse response;
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"[Execution Time] {requestName} {stopwatch.ElapsedMilliseconds} Milliseconds.");
            }
            return response;
        }
    }
}
