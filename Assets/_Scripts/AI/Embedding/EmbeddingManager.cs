using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.AI.Gemini;
using _Scripts.Diary;
using _Scripts.Embedding;
using UnityEngine;

namespace _Scripts.AI.Embedding
{
    public class EmbeddingManager : Singleton<EmbeddingManager>
    {
        private GeminiAIClient _aiClient;
        private Deboger _deboger => Deboger.Instance;

        private void Awake()
        {
            _aiClient = GeminiAiManager.Instance.GetNewAIClient(GeminiModel.TextEmbedding004);
        }

        async Task<float[]> GetEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                _deboger.LogWarning("The input text is null or empty.");
                return null;
            }

            var embedding = await _aiClient.SendEmbeddingRequestAsync(text);
            if (embedding == null)
            {
                _deboger.LogError("Getting embedding failed.");
            }

            return embedding;
        }

        public async Task<List<DiaryContainer>> SearchSimilarDiariesAsync(string query, List<DiaryContainer> allDiaries,
            int topN = 3)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<DiaryContainer>();

            var queryEmbedding = await GetEmbeddingAsync(query);

            if (queryEmbedding == null)
            {
                _deboger.LogError("Could not get embedding for query!");
                return new List<DiaryContainer>();
            }

            EmbeddingSearcher searcher = new EmbeddingSearcher();
            return searcher.FindBestMatches(allDiaries, queryEmbedding, topN);
        }

        public async Task SaveEmbeddingForDiaryAsync(DiaryContainer diary)
        {
            if (diary == null || string.IsNullOrWhiteSpace(diary.Description))
            {
                _deboger.LogWarning("Diary did not provide any description or null.");
                return;
            }

            var embedding = await GetEmbeddingAsync(diary.Description);
            if (embedding != null)
            {
                diary.SetEmbedding(embedding);
                MemoryManager.Instance.UpdateThisEditedMemoryIfExists(diary);
                _deboger.Log($"New embedding saved for : {diary.Title}");
            }
            else
                _deboger.LogError("We could not find embedding for this diary.");
        }
    }
}