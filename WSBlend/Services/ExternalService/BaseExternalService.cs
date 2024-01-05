using System.Net.Http.Headers;

namespace WSBlend.Services.ExternalService
{
    public class BaseExternalService : IDisposable
    {
        private readonly HttpClient _httpClient;

        #region CONSTRUCTOR
        public BaseExternalService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(20),
            };

            InstanciarClient();
        }

        public BaseExternalService(string url): this()
        {            
            _httpClient.BaseAddress = new Uri(url);

            InstanciarClient();
        }
        #endregion

        #region PRIVATES
        private void InstanciarClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion

        #region GET
        public async Task<string> GetJsonAsync(string path)
        {
            var response = await _httpClient.GetStringAsync(path);
            return response;
        }
        #endregion



        #region IMPLEMENTAÇÃO DISPOSE
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
        #endregion
    }
}
