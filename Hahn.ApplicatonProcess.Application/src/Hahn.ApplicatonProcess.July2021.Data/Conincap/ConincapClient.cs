using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Data.Conincap
{
    public interface IConincapClient
    {
        Task<T> CallAsync<T>(string url) where T : class;
    }

    public class ConincapClient : IConincapClient
    {
        private readonly HttpClient _httpClient;
        public ConincapClient(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("HahnClient");
        }

        public async Task<T> CallAsync<T>(string url) where T : class
        {
            HttpResponseMessage responseMessage = null;
            responseMessage = await _httpClient.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(jsonContent);
                return response;
            }
            else
            {
                throw new InvalidOperationException(
                    $"HTTP request failed. Code: {responseMessage.StatusCode}. Message: {responseMessage.ReasonPhrase}. Url: {url}.");
            }

        }
    }
}
