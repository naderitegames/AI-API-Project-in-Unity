using System.Collections.Generic;
using Nader.ObjectPooling;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Chat
{
    public class ChatManagerUI : MonoBehaviour
    {
        [SerializeField] private Transform chatContentParent;
        [SerializeField] private ChatMessageDisplayer userMessagePrefab;
        [SerializeField] private ChatMessageDisplayer aiMessagePrefab;
        [SerializeField] private ScrollRect scrollRect;

        private ObjectPool<ChatMessageDisplayer> _userMessagePool;
        private ObjectPool<ChatMessageDisplayer> _aiMessagePool;

        private void Awake()
        {
            _userMessagePool = new ObjectPool<ChatMessageDisplayer>(userMessagePrefab, 0, chatContentParent, false);
            _aiMessagePool = new ObjectPool<ChatMessageDisplayer>(aiMessagePrefab, 0, chatContentParent, false);
        }

        public void AddThisMessageToChat(ChatMessage chatMessage)
        {
            CreateOneChatSlot(chatMessage);

            // بلافاصله اسکرول کنه پایین
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void ClearChat()
        {
            _userMessagePool.DeActiveAll();
            _aiMessagePool.DeActiveAll();
        }
        
        void CreateOneChatSlot(ChatMessage chatMessage)
        {
            var messageDisplayer= chatMessage.Sender == SenderType.User ? _userMessagePool.GetObject() : _aiMessagePool.GetObject();
            messageDisplayer.transform.localScale = Vector3.one;
            messageDisplayer.transform.SetAsLastSibling();
            messageDisplayer.SetUpMessage(chatMessage);
        }
    }
}