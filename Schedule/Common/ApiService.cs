using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Schedule.Common
{
    public class ApiService : IDisposable
    {
        public ApiService()
        {
            this.Host = "http://tinchi.cyd.edu.vn";
        }
        private string Host { get; set; }

        public void Dispose()
        {
            GC.Collect();
        }

        public async Task<Result<T>> Post<T>(string url,Dictionary<string,object> data = null)
        {
            if(data == null)
            {
                data = new Dictionary<string, object>();
            }
            string json = await Task.Run(() => JsonConvert.SerializeObject(data));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                // Error here
                var httpResponse = await httpClient.PostAsync(this.Host + url, httpContent);
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Result<T>>(responseContent);
                }
                return new Result<T>();
            }
        }
    }
}