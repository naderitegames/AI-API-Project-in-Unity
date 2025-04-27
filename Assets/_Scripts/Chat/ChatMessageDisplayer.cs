using RTLTMPro;
using TMPro;
using UnityEngine;

namespace _Scripts
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