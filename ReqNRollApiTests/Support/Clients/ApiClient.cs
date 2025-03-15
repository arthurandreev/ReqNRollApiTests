using Flurl.Http;
using NLog;
using Polly;
using Polly.Retry;
using Logger = NLog.Logger;

namespace SpecflowApiTests.Support.Clients
{
    public class ApiClient : IApiClient
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string _baseUrl;
        private readonly int _timeout;
        private readonly string _authToken;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ApiClient(string baseUrl, int timeout, string authToken)
        {
            _baseUrl = baseUrl;
            _timeout = timeout;
            _authToken = authToken;

            _retryPolicy = Policy
                .Handle<FlurlHttpException>()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        logger.Warn($"Request failed. Retrying {retryCount} in {timeSpan.TotalSeconds} seconds. Error: {exception.Message}");
                    });
        }

        public async Task<IFlurlResponse> GetAsync(string endpoint)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    logger.Info($"Sending GET request to {_baseUrl + endpoint}");
                    var response = await (_baseUrl + endpoint)
                        .WithTimeout(_timeout)
                        .WithOAuthBearerToken(_authToken)
                        .GetAsync();
                    logger.Info($"Response: {response.StatusCode}");
                    return response;
                }
                catch (Exception ex)
                {
                    logger.Error($"GET request failed: {ex.Message}");
                    throw;
                }
            });
        }

        public async Task<IFlurlResponse> PostAsync(string endpoint, object data)
        {
            return (IFlurlResponse)await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    logger.Info($"Sending POST request to {_baseUrl + endpoint} with data: {data}");
                    var response = await (_baseUrl + endpoint)
                        .WithTimeout(_timeout)
                        .WithOAuthBearerToken(_authToken)
                        .PostJsonAsync(data);
                    logger.Info($"Response: {response.StatusCode}");
                    return response;
                }
                catch (Exception ex)
                {
                    logger.Error($"POST request failed: {ex.Message}");
                    throw;
                }
            });
        }

        public async Task<IFlurlResponse> PutAsync(string endpoint, object data)
        {
            return (IFlurlResponse)await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    logger.Info($"Sending PUT request to {_baseUrl + endpoint} with data: {data}");
                    var response = await (_baseUrl + endpoint)
                        .WithTimeout(_timeout)
                        .WithOAuthBearerToken(_authToken)
                        .PutJsonAsync(data);
                    logger.Info($"Response: {response.StatusCode}");
                    return response;
                }
                catch (Exception ex)
                {
                    logger.Error($"PUT request failed: {ex.Message}");
                    throw;
                }
            });
        }

        public async Task<IFlurlResponse> DeleteAsync(string endpoint)
        {
            return (IFlurlResponse)await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    logger.Info($"Sending DELETE request to {_baseUrl + endpoint}");
                    var response = await (_baseUrl + endpoint)
                        .WithTimeout(_timeout)
                        .WithOAuthBearerToken(_authToken)
                        .DeleteAsync();
                    logger.Info($"Response: {response.StatusCode}");
                    return response;
                }
                catch (Exception ex)
                {
                    logger.Error($"DELETE request failed: {ex.Message}");
                    throw;
                }
            });
        }
    }
}
