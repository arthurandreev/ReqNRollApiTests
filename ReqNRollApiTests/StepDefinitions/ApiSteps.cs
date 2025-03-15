using Flurl.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using Reqnroll;
using SpecflowApiTests.Support.Clients;
using SpecflowApiTests.Support.Configuration;

namespace SpecflowApiTests.StepDefinitions
{
    [Binding]
    public class ApiSteps
    {
        private readonly IApiClient _apiClient;
        private IFlurlResponse _response;
        private dynamic _responseData;
        private dynamic _sentData;

        public ApiSteps()
        {
            _apiClient = new ApiClient(ConfigHelper.GetBaseUrl(), ConfigHelper.GetTimeout(), ConfigHelper.GetAuthToken());
        }

        [Given("I have a valid API endpoint")]
        public void GivenIHaveAValidApiEndpoint()
        {
            Assert.IsNotNull(_apiClient);
        }

        [When("I send a GET request to (.*)")]
        public async Task WhenISendAGetRequestTo(string endpoint)
        {
            _response = await _apiClient.GetAsync(endpoint);
        }

        [When("I send a POST request to (.*) with data")]
        public async Task WhenISendAPostRequestToWithData(string endpoint, Table table)
        {
            _sentData = new
            {
                title = table.Rows[0]["title"],
                body = table.Rows[0]["body"],
                userId = int.Parse(table.Rows[0]["userId"])
            };

            _response = await _apiClient.PostAsync(endpoint, _sentData);
            _responseData = JsonConvert.DeserializeObject<dynamic>(await _response.GetStringAsync());
        }

        [When("I send a PUT request to (.*) with data")]
        public async Task WhenISendAPutRequestToWithData(string endpoint, Table table)
        {
            _sentData = new
            {
                title = table.Rows[0]["title"],
                body = table.Rows[0]["body"],
                userId = int.Parse(table.Rows[0]["userId"])
            };

            _response = await _apiClient.PutAsync(endpoint, _sentData);
            _responseData = JsonConvert.DeserializeObject<dynamic>(await _response.GetStringAsync());
        }

        [When("I send a DELETE request to (.*)")]
        public async Task WhenISendADeleteRequestTo(string endpoint)
        {
            _response = await _apiClient.DeleteAsync(endpoint);
        }

        [Then("the response data should match the sent data")]
        public void ThenTheResponseDataShouldMatchTheSentData()
        {
            Assert.AreEqual((string)_sentData.title, (string)_responseData.title, "Title does not match");
            Assert.AreEqual((string)_sentData.body, (string)_responseData.body, "Body does not match");
            Assert.AreEqual((int)_sentData.userId, (int)_responseData.userId, "User ID does not match");
        }

        [Then("the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            Assert.AreEqual(statusCode, (int)_response.StatusCode);
        }
    }
}
