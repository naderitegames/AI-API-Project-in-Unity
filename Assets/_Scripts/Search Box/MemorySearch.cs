using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;

namespace _Scripts
{
    public static class MemorySearch
    {
        public static List<DiaryContainer> SearchMemoriesByKeyword(string keyword,
            List<DiaryContainer> targetMemories)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<DiaryContainer>();
            keyword = keyword.ToLower();
            return targetMemories
                .Where(memory =>
                    (memory.Title != null && memory.Title.ToLower().Contains(keyword)) ||
                    (memory.Summary != null && memory.Summary.ToLower().Contains(keyword)) ||
                    (memory.TargetDate.ToString(CultureInfo.InvariantCulture).ToLower().Contains(keyword)) ||
                    (memory.Description != null && memory.Description.ToLower().Contains(keyword))
                )
                .ToList();
        }
    }
}