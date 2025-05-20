using System.Collections.Generic;
using _Scripts.AI.Gemini;
using _Scripts.Diary;
using UnityEngine;

namespace _Scripts.Chat
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
        private static List<ChatMessage> _messages = new List<ChatMessage>();

        public void PrepareForChat(List<DiaryContainer> targetDiaries)
        {
            _selectedDiaryContainers = targetDiaries;
        }

        public async void TrySendingMessageToAi(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput)) return;

            var userMessage = new ChatMessage(SenderType.User, userInput);
            AddMessageToChatHistory(userMessage);

            try
            {
                print(_selectedDiaryContainers.Count + " count before send");
                string prompt = ChatPromptBuilder.BuildPrompt(_selectedDiaryContainers, _messages);

                string aiResponse = await _geminiAiManager.DefaultAiClient.SendPromptAsync(prompt);

                var aiMessage = new ChatMessage(SenderType.AI, aiResponse);
                AddMessageToChatHistory(aiMessage);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error sending message to AI: " + ex.Message);
            }
        }
        public void AddMessageToChatHistory(ChatMessage newMessage)
        {
            _messages.Add(newMessage);
            chatManagerUI.AddThisMessageToChat(newMessage);
        }
        public void ResetChat()
        {
            _messages.Clear();
            //_selectedDiaryContainers.Clear();
            chatManagerUI.ClearChat(); // توی UI هم پیام ها رو پاک کن
        }
    }
}