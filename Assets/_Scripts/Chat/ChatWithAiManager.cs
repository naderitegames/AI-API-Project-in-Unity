using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts
{
    public enum SenderType
    {
        User,
        AI
    }

    public class ChatWithAiManager : MonoBehaviour
    {
        private GeminiAiManager _geminiAiManager => GeminiAiManager.Instance;
        [SerializeField] private ChatManagerUI chatManagerUI;

        private List<DiaryContainer> _selectedDiaryContainers = new List<DiaryContainer>();
        private List<ChatMessage> _messages = new List<ChatMessage>();

        public void PrepareForChat(List<DiaryContainer> targetDiaries)
        {
            _selectedDiaryContainers = targetDiaries;
        }

        public void AddMessageToChat(ChatMessage newMessage)
        {
            _messages.Add(newMessage);
            chatManagerUI.AddThisMessageToChat(newMessage);
        }

        public async void TrySendingMessageToAi(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput)) return;

            var userMessage = new ChatMessage(SenderType.User, userInput);
            AddMessageToChat(userMessage);

            try
            {
                string prompt = ChatPromptBuilder.BuildPrompt(_selectedDiaryContainers, _messages);

                string aiResponse = await _geminiAiManager.AIClient.SendPromptAsync(prompt);

                var aiMessage = new ChatMessage(SenderType.AI, aiResponse);
                AddMessageToChat(aiMessage);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error sending message to AI: " + ex.Message);
            }
        }

        /*private string BuildContextFromSelectedDiaries()
        {
            StringBuilder contextBuilder = new StringBuilder();
            foreach (var diary in _selectedDiaryContainers)
            {
                contextBuilder.AppendLine($"عنوان خاطره: {diary.Title}\nتوضیحات خااطره: {diary.Description}\n--- این هارو در نظر بگیر و با توجه به این خاطره ها پاسخ بده.\n\n");
            }

            return contextBuilder.ToString();
        }*/
        public void ResetChat()
        {
            _messages.Clear(); // لیست پیام‌ها رو خالی کن
            _selectedDiaryContainers.Clear(); // دیاری‌های انتخاب شده رو هم خالی کن (اختیاری، اگه نمیخوای پاک شه اینو بردار)
            chatManagerUI.ClearChat(); // توی UI هم پیام ها رو پاک کن
        }
    }
}