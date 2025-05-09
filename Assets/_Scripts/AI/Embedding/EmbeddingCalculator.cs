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
        /*public static float CosineSimilarity(float[] a, float[] b)
        {
            float dot = 0f, magA = 0f, magB = 0f;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }
            if (magA == 0 || magB == 0) return 0f;
            return dot / (Mathf.Sqrt(magA) * Mathf.Sqrt(magB));
        }*/
    }
}