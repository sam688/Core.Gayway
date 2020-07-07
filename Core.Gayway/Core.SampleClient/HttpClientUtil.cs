using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.SampleClient
{
    public class HttpClientUtil
    {
        public static string Get(string url)
        {
            using(HttpClient httpClient=new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();

                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var responseMessage = httpClient.SendAsync(message).Result;
                string result = responseMessage.Content.ReadAsStringAsync().Result;
                return result;
            }
        }
    }
}
