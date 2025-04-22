using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public enum GeminiModel
    {
        Gemini2_0Flash,
        Gemini2_0FlashLite,
        Gemini1_5Pro,
        Gemini1_5Flash,
        Gemini1_5Flash8B,
        TextEmbedding004,
        Aqa
    }

    public class GeminiAiManager : MonoBehaviour
    {
        [SerializeField] string apiKey;
        private GeminiAIClient _aiClient;
        public GeminiAIClient AIClient => _aiClient;

        [SerializeField] GeminiModel selectedGeminiModel;

        private static readonly Dictionary<GeminiModel, string> ModelMap = new Dictionary<GeminiModel, string>
        {
            { GeminiModel.Gemini2_0Flash, "models/gemini-2.0-flash" },
            { GeminiModel.Gemini2_0FlashLite, "models/gemini-2.0-flash-lite" },
            { GeminiModel.Gemini1_5Pro, "models/gemini-1.5-pro" },
            { GeminiModel.Gemini1_5Flash, "models/gemini-1.5-flash" },
            { GeminiModel.Gemini1_5Flash8B, "models/gemini-1.5-flash-8b" },
            { GeminiModel.TextEmbedding004, "models/text-embedding-004" },
            { GeminiModel.Aqa, "models/aqa" }
        };

        private void Awake()
        {
            _aiClient = new GeminiAIClient(apiKey, ModelMap[selectedGeminiModel]);
        }
    }
}