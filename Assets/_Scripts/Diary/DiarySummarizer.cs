using System;
using _Scripts.AI.Gemini;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Diary
{
    public static class DiarySummarizer
    {
        public static bool CanSummarize(DiaryContainer diary, UiManager ui)
        {
            if (string.IsNullOrWhiteSpace(diary.Description))
            {
                ui.DisplayThisWarning("متن خاطره خالی است و قابل خلاصه‌سازی نیست.", "بسیار خب");
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

        public static void TrySummarize(string target, int maxLenght, Action<string> onSummaryReady = null)
        {
            UiManager.Instance.DisplayThisWarning("کمی صبر کنید");
            SummarizeAsync(target, maxLenght, onSummaryReady);
        }

        private static async void SummarizeAsync(
            string text,
            int summaryLine,
            Action<string> onSummaryReady)
        {
            try
            {
                string prompt = $"این متن را در " +
                                $"{summaryLine}" +
                                " خط خلاصه کن طوری که انگار خود کاربر این متن رو خلاصه نوشته :" +
                                $"\n\n{text}";
                string newSummary = await AIClient.SendPromptAsync(prompt);
                if (!string.IsNullOrWhiteSpace(newSummary))
                {
                    onSummaryReady?.Invoke(newSummary);
                    UiManager.Instance.DisplayThisWarning("متن شما از هوش مصنوعی دریافت شد!", "بسیار خب");
                }
                else
                {
                    UiManager.Instance.DisplayThisWarning("پاسخی از هوش مصنوعی دریافت نشد.", "بسیار خب");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("❌ خطا در خلاصه‌سازی: " + e.Message);
                UiManager.Instance.DisplayThisWarning("مشکلی در ارتباط با AI پیش آمد.", "سیار خب");
            }
        }
    }
}