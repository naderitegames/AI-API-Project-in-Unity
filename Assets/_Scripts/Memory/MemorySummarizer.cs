using System;
using UnityEngine;

namespace _Scripts
{
    public static class MemorySummarizer
    {
        public static bool CanSummarize(MemoryContainer memory, UiManager ui)
        {
            if (string.IsNullOrWhiteSpace(memory.Description))
            {
                ui.DisplayThisWarning("متن خاطره خالی است و قابل خلاصه‌سازی نیست.","بسیار خب");
                return false;
            }

            return true;
        }

        public static void TrySummarizeWithConfirmation(
            MemoryContainer memory,
            GeminiAiManager ai,
            UiManager ui,
            int summarizeLine,
            Action<string> onSummaryReady)
        {
            if (string.IsNullOrWhiteSpace(memory.Summary))
            {
                SummarizeAsync(memory, ai, ui, summarizeLine, onSummaryReady);
            }
            else
            {
                ui.DisplayThisWarning(
                    "این خاطره قبلاً خلاصه شده. دوباره خلاصه شود؟",
                    () =>
                    {
                        SummarizeAsync(memory, ai, ui, summarizeLine, onSummaryReady);
                    },
                    "نه، بی‌خیال",
                    "آره، دوباره خلاصه کن"
                );
            }
        }

        private static async void SummarizeAsync(
            MemoryContainer memory,
            GeminiAiManager ai,
            UiManager ui,
            int summaryLine,
            Action<string> onSummaryReady)
        {
            try
            {
                ui.DisplayThisWarning("کمی صبر کنید");
                string prompt = $"این متن را در " +
                                $"{summaryLine}" +
                                " خط خلاصه کن و حرف اضافه ای نزن که معلوم بشه AI اینکارو کرده :" +
                                $"\n\n{memory.Description}";
                string newSummary = await ai.AIClient.SendPromptAsync(prompt);
                if (!string.IsNullOrWhiteSpace(newSummary))
                {
                    onSummaryReady?.Invoke(newSummary);
                    ui.DisplayThisWarning("متن شما از هوش مصنوعی دریافت شد!","بسیار خب");

                }
                else
                {
                    ui.DisplayThisWarning("پاسخی از هوش مصنوعی دریافت نشد.","بسیار خب");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("❌ خطا در خلاصه‌سازی: " + e.Message);
                ui.DisplayThisWarning("مشکلی در ارتباط با AI پیش آمد.","سیار خب");
            }
        }
    }
}