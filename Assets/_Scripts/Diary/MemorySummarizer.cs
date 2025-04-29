using System;
using _Scripts.AI.Gemini;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Diary
{
    public static class MemorySummarizer
    {
        public static bool CanSummarize(DiaryContainer diary, UiManager ui)
        {
            if (string.IsNullOrWhiteSpace(diary.Description))
            {
                ui.DisplayThisWarning("متن خاطره خالی است و قابل خلاصه‌سازی نیست.","بسیار خب");
                return false;
            }

            return true;
        }

        private static GeminiAIClient _aIClient;

        private static GeminiAIClient AIClient
        {
            get
            {
                if (_aIClient == null)
                    _aIClient = GeminiAiManager.Instance.GetNewAIClient(GeminiModel.Gemini2_0Flash);
                return _aIClient;
            }
        }
        public static void TrySummarizeWithConfirmation(
            DiaryContainer diary,
            UiManager ui,
            int summarizeLine,
            Action<string> onSummaryReady)
        {
            if (string.IsNullOrWhiteSpace(diary.Summary))
            {
                SummarizeAsync(diary, ui, summarizeLine, onSummaryReady);
            }
            else
            {
                ui.DisplayThisWarning(
                    "این خاطره قبلاً خلاصه شده. دوباره خلاصه شود؟",
                    () =>
                    {
                        SummarizeAsync(diary, ui, summarizeLine, onSummaryReady);
                    },
                    "نه، بی‌خیال",
                    "آره، دوباره خلاصه کن"
                );
            }
        }

        private static async void SummarizeAsync(
            DiaryContainer diary,
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
                                $"\n\n{diary.Description}";
                string newSummary = await AIClient.SendPromptAsync(prompt);
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