using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Utilities
{
    public class HttpRequester
    {
        public static async Task<TType> SendRequestAsync<TType>(string requestUrl, string token)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response  = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TType>(responseContent);
            }
            return default(TType);
        }

        public static async Task<TType2> PostObject<TType, TType2>(TType postObject, string requestUrl, Dictionary<string, string> headers)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(postObject));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                var httpResponse = await httpClient.PostAsync(requestUrl, httpContent);
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    var response = await Task.Run(() => JsonConvert.DeserializeObject<TType2>(responseContent));
                    return response;
                }
            }
            return default(TType2);
        }
    }
}
