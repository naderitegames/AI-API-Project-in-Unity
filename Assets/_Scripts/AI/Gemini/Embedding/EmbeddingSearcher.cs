using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.AI.Embedding;
using _Scripts.Diary;
using UnityEngine;

namespace _Scripts.Embedding
{
    public class EmbeddingSearcher
    {
        public List<DiaryContainer> FindBestMatches(List<DiaryContainer> diaries, float[] searchEmbedding)
        {
            var scoredDiaries = diaries
                .Where(d => d.HasEmbedding())
                .Select(d =>
                {
                    var score = EmbeddingCalculator.CosineSimilarity(d.Embedding, searchEmbedding);
                    //Debug.Log($"Diary: {d.Title.faConvert()} | Similarity: {score}");
                    return (diary: d, score);
                })
                .OrderByDescending(x => x.score) // مرتب‌سازی از بیشترین به کمترین
                //.Take(2) // گرفتن 2 خاطره
                .Select(x => x.diary)
                .ToList();

            //Debug.Log(scoredDiaries.Count + " found");
            return scoredDiaries;
        }

    }
}