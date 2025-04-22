using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts
{
    public class GeminiAIClient
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _httpClient;

        public GeminiAIClient(string apiKey, string model = "models/gemini-pro")
        {
            _apiKey = apiKey;
            _model = model;
            _httpClient = new HttpClient();
        }

        private string ApiUrl =>
            $"https://generativelanguage.googleapis.com/v1beta/{_model}:generateContent?key={_apiKey}";

        public async Task<string> SendPromptAsync(string prompt)
        {
            string jsonPayload = JsonUtility.ToJson(new GeminiRequest(prompt));

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(ApiUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError($"❌ Gemini API error: {response.StatusCode}");
                    return $"خطا: {response.StatusCode}";
                }

                var responseText = await response.Content.ReadAsStringAsync();
                return ParseResponse(responseText);
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Exception while calling Gemini API: " + ex.Message);
                return "خطا در ارتباط با سرور AI.";
            }
        }

        private string ParseResponse(string json)
        {
            try
            {
                var parsed = JsonUtility.FromJson<GeminiRawResponse>(json);
                return parsed?.candidates[0].content.parts[0].text ?? "پاسخی دریافت نشد.";
            }
            catch (Exception e)
            {
                Debug.LogError("❌ JSON parse error: " + e.Message);
                return "خطا در پردازش پاسخ.";
            }
        }

        [Serializable]
        private class GeminiRequest
        {
            public GeminiRequest(string prompt)
            {
                contents = new[]
                {
                    new Content
                    {
                        parts = new[]
                        {
                            new Part { text = prompt }
                        }
                    }
                };
            }

            public Content[] contents;
        }

        [Serializable]
        private class Content
        {
            public Part[] parts;
        }

        [Serializable]
        private class Part
        {
            public string text;
        }

        [Serializable]
        private class GeminiRawResponse
        {
            public Candidate[] candidates;
        }

        [Serializable]
        private class Candidate
        {
            public Content content;
        }
    }
}