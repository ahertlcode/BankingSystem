using System;


namespace BankingSystem.Tests.Helpers
{
    public static class HttpClientExtensions
    {   
        public static async Task<T?> GetJson<T>(this HttpClient client, string url) =>
            await client.GetFromJsonAsync<T>(url);

        public static async Task<HttpResponseMessage> PostJson<T>(
            this HttpClient client, string url, T payload) =>
            await client.PostAsJsonAsync(url, payload);
    }

}
