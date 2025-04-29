using RTLTMPro;
using UnityEngine;

namespace _Scripts.Chat
{
    public class ChatMessageDisplayer : MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro messageText;
        private ChatMessage _chatMessage;

        public void SetUpMessage(ChatMessage chatMessage)
        {
            _chatMessage = chatMessage;
            messageText.text = chatMessage.Message;
        }
    }
}