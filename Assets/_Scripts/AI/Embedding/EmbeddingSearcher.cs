using System.Collections.Generic;
using System.Linq;
using _Scripts.AI.Embedding;
using _Scripts.Diary;
using UnityEngine;

namespace _Scripts.Embedding
{
    public class EmbeddingSearcher
    {
        public List<DiaryContainer> FindBestMatches(List<DiaryContainer> diaries, float[] searchEmbedding, int topN)
        {
            var similarities = new List<(DiaryContainer diary, float score)>();

            foreach (var diary in diaries)
            {
                if (!diary.HasEmbedding()) continue;

                float similarity = EmbeddingCalculator.CosineSimilarity(diary.Embedding, searchEmbedding);
                similarities.Add((diary, similarity));

                Debug.Log($"📘 Diary: {diary.Title} | Similarity: {similarity:F4}");
            }

            var topMatches = similarities
                .OrderByDescending(x => x.score)
                .Take(topN)
                .Select(x => x.diary)
                .ToList();

            return topMatches;
        }
    }

}