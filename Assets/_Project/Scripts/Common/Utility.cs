using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VirtueSky.Threading.Tasks;

namespace TheBeginning.Common
{
    public static class Utility
    {
        public static readonly HttpClient Client = new HttpClient();

        public static async UniTask<string> TranslateAsync(string text, string targetLanguage,
            string sourceLanguage = "auto")
        {
            var url =
                $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";

            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            Debug.LogWarning(response.EnsureSuccessStatusCode());

            string responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JArray.Parse(responseBody);
            return (string)responseJson[0][0]?[0];
        }
    }
}