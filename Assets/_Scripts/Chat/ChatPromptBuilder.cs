using System.Collections.Generic;
using System.Globalization;
using System.Text;
using _Scripts.Diary;

namespace _Scripts.Chat
{
    public static class ChatPromptBuilder
    {
        public static string BuildPrompt(List<DiaryContainer> diaries, List<ChatMessage> messages)
        {
            StringBuilder promptBuilder = new StringBuilder();
            // اول کانتکست دیاری ها
            PrepareSelectedDiaries();
            // پیام هارو اضافه کن
            PrepareMessages();
            // حرف های اضافه رو اضافه کن
            PrepareExtraWords();
            // آماده‌ی ارسال
            return promptBuilder.ToString();

            void PrepareSelectedDiaries()
            {
                foreach (var diary in diaries)
                {
                    promptBuilder.AppendLine("عنوان خاطره: " + $" {diary.Title}\n\n");
                    promptBuilder.AppendLine("توضیحات خاطره: " + $" {diary.Description}\n\n");
                    promptBuilder.AppendLine("زمان ایجاد خاطره: " +
                                             $" {diary.CreationDate.ToString(CultureInfo.InvariantCulture)}\n\n");
                    promptBuilder.AppendLine("زمان آخرین تغییر خاطره: " + $" {diary.LastUpdateTime}\n\n");
                }
            }

            void PrepareMessages()
            {
                promptBuilder.AppendLine("و مکالمه قبلی بین کاربر و هوش مصنوعی: ");

                // بعد پیام‌های چت قبلی
                foreach (var message in messages)
                {
                    string rolePrefix = message.Sender == SenderType.User ? "کاربر" : "هوش مصنوعی";
                    promptBuilder.AppendLine($"{rolePrefix} : {message.Message}\n\n");
                }
            }

            void PrepareExtraWords()
            {
                promptBuilder.AppendLine("حالا جواب های بعدی رو طوری بده که معلوم نباشه AI از طرف ai داری حرف میزنی.");
                promptBuilder.AppendLine(
                    "و قبل از پیامت نیازی به استفاده از کلماتی مثل AI یا اشاره به این که جواب از سمت AI است." + "\n\n");
            }
        }
    }
}