using System;

namespace _Scripts.Chat
{
    [Serializable]
    public class ChatMessage
    {
        public SenderType Sender => _sender;
        SenderType _sender;
        public string Message => _message;
        string _message;
        public DateTime CreationTime => _creationTime;
        private DateTime _creationTime;

        public ChatMessage(SenderType sender, string message)
        {
            _sender = sender;
            _message = message;
            _creationTime = DateTime.Now;
        }

        private ChatMessage()
        {
            
        }
    }
}