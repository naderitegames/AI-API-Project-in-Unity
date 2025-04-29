using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Chat
{
    public class UserChatInputField : MonoBehaviour
    {
        [SerializeField] private ChatWithAiManager chatWithAiManager;
        [SerializeField] private TMP_InputField userInputField;
        [SerializeField] private Button submitButton;

        private void OnEnable()
        {
            submitButton.onClick.AddListener(OnSubmitButtonClicked);
        }

        private void OnDisable()
        {
            submitButton.onClick.RemoveListener(OnSubmitButtonClicked);
        }

        private void OnSubmitButtonClicked()
        {
            if (string.IsNullOrEmpty(userInputField.text)) return;

            chatWithAiManager.TrySendingMessageToAi(userInputField.text);

            userInputField.text = ""; // پاک کردن ورودی بعد از ارسال
        }
    }
}