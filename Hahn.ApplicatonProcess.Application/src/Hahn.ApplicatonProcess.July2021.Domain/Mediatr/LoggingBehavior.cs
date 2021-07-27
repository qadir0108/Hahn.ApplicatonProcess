using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hahn.ApplicatonProcess.July2021.Domain.Mediatr
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var requestJson = JsonConvert.SerializeObject(request, Formatting.Indented, jsonSettings);

            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            _logger.LogInformation("{RequestBody}", requestJson);

            try
            {
                var response = await next();
                var responseJson = JsonConvert.SerializeObject(response, Formatting.Indented, jsonSettings);
                _logger.LogInformation($"Handled {typeof(TResponse).Name}");
                _logger.LogInformation("{ResponseBody}", responseJson);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Error}");
                throw;
            }
        }
    }
}