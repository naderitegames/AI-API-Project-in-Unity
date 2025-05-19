using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Diary;
using _Scripts.Embedding;

namespace _Scripts.AI.Gemini.Embedding
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
            var embedding = await _aiClient.SendEmbeddingRequestAsync(text.faConvert());
            print("target req = "+text.faConvert());
            if (embedding == null)
            {
                _deboger.LogError("Getting embedding failed.");
            }

            return embedding;
        }

        public async Task<List<DiaryContainer>> SearchSimilarDiariesAsync(string query, List<DiaryContainer> allDiaries,
            int topN = 2)
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
            var results = searcher.FindBestMatches(allDiaries, queryEmbedding, topN);
            //print(results.Count + " this will be send");
            return results;
        }

        public async Task SaveEmbeddingForDiaryAsync(DiaryContainer diary)
        {
            if (diary == null || string.IsNullOrWhiteSpace(diary.Description) && string.IsNullOrWhiteSpace(diary.Title))
            {
                _deboger.LogWarning("Diary did not provide any description or null.");
                return;
            }

            var embedding = await GetEmbeddingAsync(diary.Description + " " + diary.Title);
            if (embedding != null)
            {
                diary.SetEmbedding(embedding);
                //print($"Embedding Length: {embedding.Length}"); // باید مثلاً 768 یا 1024 باشه
                MemoryManager.Instance.UpdateThisEditedMemoryIfExists(diary);
                _deboger.Log($"New embedding saved for : {diary.Title}");
            }
            else
                _deboger.LogError("We could not find embedding for this diary.");
        }
    }
}