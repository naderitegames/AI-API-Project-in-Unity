using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.AI.Gemini
{
    public class GeminiAIClient
    {
        private string ApiUrl => _apiURL;
        private readonly string _apiURL;
        private readonly string _apiKey;
        private readonly GeminiModel _model;
        private readonly HttpClient _httpClient;
        private Deboger _deboger => Deboger.Instance;

        private bool IsEmbeddingModel =>
            GeminiAiManager.Instance.GetThisModelName(_model) ==
            GeminiAiManager.Instance.GetThisModelName(GeminiModel.TextEmbedding004);

        public GeminiAIClient(string apiKey, GeminiModel model)
        {
            _apiKey = apiKey;
            _model = model;
            _httpClient = new HttpClient();

            string baseUrl = "https://generativelanguage.googleapis.com/v1beta/";
            string targetModel = GeminiAiManager.Instance.GetThisModelName(_model);
            string endpoint = IsEmbeddingModel ? ":embedContent" : ":generateContent";
            _apiURL = $"{baseUrl}{targetModel}{endpoint}?key={_apiKey}";
        }

        // متد برای گرفتن embedding
        public async Task<float[]> SendEmbeddingRequestAsync(string text)
        {
            // ساخت یک نمونه از این کلاس و تبدیل به فرمت JSON
            string jsonPayload = JsonUtility.ToJson(new GeminiEmbeddingRequest(text));

            _deboger.Log(ApiUrl);
            _deboger.Log("Payload: " + jsonPayload);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(ApiUrl, content); //ارسال درخواست آماده شده به API 
                if (!response.IsSuccessStatusCode) //اگر موفق نبود 
                {
                    _deboger.ForceLogError($"Embedding API error: {response.StatusCode}");
                    return null;
                }

                var responseText = await response.Content.ReadAsStringAsync();
                return ParseResponseForEmbedding(responseText);
            }
            catch (Exception ex)
            {
                _deboger.ForceLogError("Exception in embedding request: " + ex.Message);
                return null;
            }
        }

        public async Task<string> SendPromptAsync(string prompt) // متد برای گرفتن پاسخ متنی معمولی 
        {
            string jsonPayload = JsonUtility.ToJson(new GeminiRequest(prompt));
            _deboger.Log(ApiUrl);
            _deboger.Log("Payload: " + jsonPayload);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(ApiUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    _deboger.ForceLogError($"Gemini API error: {response.StatusCode}");
                    return $"خطا: {response.StatusCode}";
                }

                var responseText = await response.Content.ReadAsStringAsync();
                return ParseResponse(responseText);
            }
            catch (Exception e)
            {
                _deboger.ForceLogError("Exception while calling Gemini API: " + e.Message);
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
                _deboger.ForceLogError("JSON parse error: " + e.Message);
                return "خطا در پردازش پاسخ.";
            }
        }

        private float[] ParseResponseForEmbedding(string response)
        {
            try
            {
                var result = JsonUtility.FromJson<EmbeddingResponse>(response);
                return result.embedding?.values?.ToArray();
            }
            catch (Exception ex)
            {
                _deboger.ForceLogError("Parse error: " + ex.Message);
                _deboger.ForceLogError("Raw embedding response: " + response);
                return null;
            }
        }

        // ---------- JSON Models ----------

        [Serializable]
        private class GeminiRequest
        {
            public List<Content> contents;

            public GeminiRequest(string prompt)
            {
                contents = new List<Content>
                {
                    new Content
                    {
                        parts = new Part[] { new Part { text = prompt } }
                    }
                };
            }
        }

        [Serializable]
        private class GeminiEmbeddingRequest
        {
            public Content content;

            public GeminiEmbeddingRequest(string prompt)
            {
                content = new Content
                {
                    parts = new Part[] { new Part { text = prompt } }
                };
            }
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

        [Serializable]
        public class EmbeddingResponse
        {
            public Embedding embedding;
        }

        [Serializable]
        public class Embedding
        {
            public List<float> values;
        }
    }
}