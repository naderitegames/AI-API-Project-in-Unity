using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.AI.Gemini
{
    public enum GeminiModel
    {
        Gemini2_0Flash=0,
        Gemini2_0FlashLite,
        Gemini1_5Pro,
        Gemini1_5Flash,
        Gemini1_5Flash8B,
        Aqa,
        TextEmbedding004,
        Gemini_embedding_exp_03_07=7
    }

    public class GeminiAiManager : Singleton<GeminiAiManager>
    {
        [SerializeField] string apiKey;
        private GeminiAIClient _defaultAiClient;
        public GeminiAIClient DefaultAiClient => _defaultAiClient;

        [SerializeField] GeminiModel selectedGeminiModel;

        private static readonly Dictionary<GeminiModel, string> ModelMap = new Dictionary<GeminiModel, string>
        {
            { GeminiModel.Gemini2_0Flash, "models/gemini-2.0-flash" },
            { GeminiModel.Gemini2_0FlashLite, "models/gemini-2.0-flash-lite" },
            { GeminiModel.Gemini1_5Pro, "models/gemini-1.5-pro" },
            { GeminiModel.Gemini1_5Flash, "models/gemini-1.5-flash" },
            { GeminiModel.Gemini1_5Flash8B, "models/gemini-1.5-flash-8b" },
            { GeminiModel.TextEmbedding004, "models/text-embedding-004" },
            { GeminiModel.Gemini_embedding_exp_03_07, "models/gemini-embedding-exp-03-07" },
            { GeminiModel.Aqa, "models/aqa" }
        };

        public override void Awake()
        {
            _defaultAiClient = new GeminiAIClient(apiKey, selectedGeminiModel);
        }

        public GeminiAIClient GetNewAIClient(GeminiModel model)
        {
            return new GeminiAIClient(apiKey, model);
        }

        public string GetThisModelName(GeminiModel target)
        {
            return ModelMap[target];
        }
    }
}