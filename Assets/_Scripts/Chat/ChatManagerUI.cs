using UnityEngine;
using Nader.ObjectPooling;
using UnityEngine.UI;

namespace _Scripts
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
            _userMessagePool = new ObjectPool<ChatMessageDisplayer>(userMessagePrefab, 5, chatContentParent, false);
            _aiMessagePool = new ObjectPool<ChatMessageDisplayer>(aiMessagePrefab, 5, chatContentParent, false);
        }

        public void AddThisMessageToChat(ChatMessage chatMessage)
        {
            ChatMessageDisplayer messageDisplayer = chatMessage.Sender == SenderType.User
                ? _userMessagePool.GetObject()
                : _aiMessagePool.GetObject();
            messageDisplayer.transform.SetAsLastSibling();
            messageDisplayer.SetUpMessage(chatMessage);

            // بلافاصله اسکرول کنه پایین
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void ClearChat()
        {
            _userMessagePool.DeActiveAll();
            _aiMessagePool.DeActiveAll();
        }
    }
}