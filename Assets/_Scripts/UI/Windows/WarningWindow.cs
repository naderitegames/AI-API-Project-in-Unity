using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class WarningWindow : WindowPanel
    {
        [SerializeField] RTLTextMeshPro warningText;
        [SerializeField] RTLTextMeshPro cancelText;
        [SerializeField] RTLTextMeshPro okText;
        [SerializeField] Button targetOkButton;
        [SerializeField] Button targetCancelBtton;
        Action _onOkClicked;

        private void OnEnable()
        {
            targetOkButton.onClick.AddListener(OnOkButtonClicked);
            targetCancelBtton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnDisable()
        {
            targetOkButton.onClick.RemoveListener(OnOkButtonClicked);
            targetCancelBtton.onClick.RemoveListener(OnCancelButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            SetActive(false);
            _onOkClicked?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            SetActive(false);
        }

        public void UpdateTextAndDisplay(string message, Action ifPressOk, string cancelButtonText = "Cancel",
            string okButtonText = "Continue")
        {
            targetCancelBtton.gameObject.SetActive(true);
            targetOkButton.gameObject.SetActive(true);
            warningText.text = message;
            okText.text = okButtonText;
            cancelText.text = cancelButtonText;
            _onOkClicked = ifPressOk;
            SetActive(true);
        }

        public void UpdateTextAndDisplay(string message, string text = "بسیار خب",
            string okButtonText = "Continue")
        {
            targetCancelBtton.gameObject.SetActive(false);
            targetOkButton.gameObject.SetActive(true);
            warningText.text = message;
            okText.text = text;
            _onOkClicked = null;
            SetActive(true);
        }
        public void UpdateTextAndDisplay(string message)
        {
            targetCancelBtton.gameObject.SetActive(false);
            targetOkButton.gameObject.SetActive(false);
            warningText.text = message;
            SetActive(true);
        }
    }
}