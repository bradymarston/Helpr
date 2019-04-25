using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Helpr.Client.Services
{
    public class ValuesService
    {
        private readonly HttpClient _http;

        public ValuesService(HttpClient http)
        {
            _http = http;
        }
        public async Task<string[]> GetValuesAsync()
        {
            var response = await _http.GetAsync("api/values");
            var list = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());
            return list;
        }
    }
}