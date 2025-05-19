using UnityEngine;

namespace _Scripts.AI.Embedding
{
    public static class EmbeddingCalculator
    {
        public static float CosineSimilarity(float[] a, float[] b)
        {
            float dot = 0f;
            float normA = 0f;
            float normB = 0f;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }

            return dot / (Mathf.Sqrt(normA) * Mathf.Sqrt(normB) + 1e-10f); // جلوگیری از تقسیم بر صفر
        }
    }
}