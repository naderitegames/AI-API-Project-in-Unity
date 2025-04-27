using System.Collections.Generic;
using System.Text;

namespace _Scripts
{
    public static class ChatPromptBuilder
    {
        public static string BuildPrompt(List<DiaryContainer> diaries, List<ChatMessage> messages)
        {
            StringBuilder promptBuilder = new StringBuilder();

            // اول کانتکست دیاری ها
            foreach (var diary in diaries)
            {
                promptBuilder.AppendLine($"عنوان خاطره: {diary.Title}\nتوضیحات خاطره: {diary.Description}\n---\n");
            }

            promptBuilder.AppendLine("مکالمه قبلی بین کاربر و هوش مصنوعی:");
            
            // بعد پیام‌های چت قبلی
            foreach (var message in messages)
            {
                string rolePrefix = message.Sender == SenderType.User ? "User" : "Assistant";
                promptBuilder.AppendLine($"{rolePrefix}: {message.Message}");
            }

            // آماده‌ی ارسال
            return promptBuilder.ToString();
        }
    }
}