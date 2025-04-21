using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace _Scripts
{
    public static class MemorySearch
    {
        public static List<MemoryContainer> SearchMemoriesByKeyword(string keyword, List<MemoryContainer> targetMemories)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<MemoryContainer>();

            keyword = keyword.ToLower();

            return targetMemories
                .Where(memory =>
                    (memory.Title != null && memory.Title.ToLower().Contains(keyword)) ||
                    (memory.Summary != null && memory.Summary.ToLower().Contains(keyword)) ||
                    (memory.Description != null && memory.Description.ToLower().Contains(keyword))
                )
                .ToList();
        }
    }
}