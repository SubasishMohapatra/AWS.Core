using Polly;
using Serilog;

namespace AWS.SharedLib
{
    public class PollyRetry
    {
        //Create a service for Polly retry and implement the generci solution for both httpclient and s3client
        public async static Task<TResult> ExecuteAsync<TResult>(
        Func<Task<TResult>> actionFunc,
        Func<DelegateResult<TResult>, Context, CancellationToken, Task<TResult>> fallback,
        Func<DelegateResult<TResult>, Context, Task> onFallbackAsync, ILogger logger)
        {
            var fallbackPolicy = Policy<TResult>.Handle<Exception>()
                                    .FallbackAsync(fallback, onFallbackAsync);

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                   (ex, timeSpan, retryCount, context) =>
                   {
                       // Log exception
                       logger.Error(ex, "Retry attempt: {RetryCount} failed. Retrying in {Time} seconds.", retryCount, timeSpan);
                   });
            var policyResult = await fallbackPolicy.WrapAsync(retryPolicy).ExecuteAndCaptureAsync(actionFunc);
            return policyResult.Result;
        }
    }
}