using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.AI.Gemini.Embedding;
using _Scripts.Diary;
using _Scripts.UI;

namespace _Scripts.Search_Box
{
    public static class MemorySearch
    {
        public static List<DiaryContainer> SearchMemoriesByKeyword(string keyword, List<DiaryContainer> targetMemories)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<DiaryContainer>();
            keyword = keyword.ToLower();
            return targetMemories
                .Where(memory =>
                    (memory.Title != null && memory.Title.ToLower().Contains(keyword)) ||
                    (memory.Summary != null && memory.Summary.ToLower().Contains(keyword)) ||
                    (memory.CreationDate.ToString(CultureInfo.InvariantCulture).ToLower().Contains(keyword)) ||
                    (memory.Description != null && memory.Description.ToLower().Contains(keyword))
                )
                .ToList();
        }

        public static async Task<List<DiaryContainer>> SearchMemoriesByEmbedding(string keyword,
            List<DiaryContainer> targetMemories)
        {
            UiManager.Instance.DisplayThisWarning("در حال برسی اطلاعات. . .");
            foreach (var diary in targetMemories) // بررسی اینکه آیا هر Diary دارای embedding است یا نه 
            {
                if (!diary.HasEmbedding())
                {
                    // دریافت و ذخیره embedding برای این diary
                    await EmbeddingManager.Instance.SaveEmbeddingForDiaryAsync(diary);
                }
            }

            UiManager.Instance.DisplayThisWarning("در حال ارسال اطلاعات. کمی صبر کنید");

            // حالا جستجو با AI رو انجام میدیم
            var results =
                await EmbeddingManager.Instance.SearchSimilarDiariesAsync(keyword, targetMemories);
            UiManager.Instance.CloseWarningWindow();
            return results;
        }
    }
}