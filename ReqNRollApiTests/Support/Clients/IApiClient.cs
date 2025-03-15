using Flurl.Http;

namespace SpecflowApiTests.Support.Clients
{
    public interface IApiClient
    {
        Task<IFlurlResponse> GetAsync(string endpoint);
        Task<IFlurlResponse> PostAsync(string endpoint, object data);
        Task<IFlurlResponse> PutAsync(string endpoint, object data);
        Task<IFlurlResponse> DeleteAsync(string endpoint);
    }
}
