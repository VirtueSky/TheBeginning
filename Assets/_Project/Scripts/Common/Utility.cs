using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using VirtueSky.Threading.Tasks;

namespace TheBeginning
{
    public static class Utility
    {
        public static Dictionary<string, AsyncOperationHandle<SceneInstance>> sceneHolder =
            new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

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