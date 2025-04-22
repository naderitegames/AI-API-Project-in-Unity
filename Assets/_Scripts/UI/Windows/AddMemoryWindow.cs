using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class AddMemoryWindow : WindowPanel
    {
        [SerializeField] private MemoryManager _manager;
        MemoryContainer _targetMemory;
        [SerializeField] TMP_InputField descriptopnInputField;
        [SerializeField] TMP_InputField titleInputField;
        [SerializeField] Button targetSubmitButton;
        [SerializeField] Button targetClearButton;
        [SerializeField] MemoryInput _memoryInput;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            targetSubmitButton.onClick.AddListener(TrySubmitNewMemory);
            targetClearButton.onClick.AddListener(ClearMemoryDescription);

            descriptopnInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            titleInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
        }

        private void OnDisable()
        {
            targetSubmitButton.onClick.RemoveListener(TrySubmitNewMemory);
            targetClearButton.onClick.RemoveListener(ClearMemoryDescription);

            descriptopnInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
            titleInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
        }

        void TrySubmitNewMemory()
        {
            UiManager.Instance.DisplayThisWarning(
                "از اضافه شدن این خاطره اطمینان دارید؟!",
                () =>
                {
                    _manager.CreateMemoryFromInput(_memoryInput);
                    UiManager.Instance.ActivatePanel(WindowType.Home);
                },
                "خیر",
                "بله, ثبت کن");
        }

        void ClearMemoryDescription()
        {
            _memoryInput.ClearDescription();
        }

        void OnOnInputFieldsChange(string arg0)
        {
            UpdateSaveButtonInteractable();
        }

        void UpdateSaveButtonInteractable()
        {
            targetSubmitButton.interactable = descriptopnInputField.text != "";
            targetClearButton.interactable = descriptopnInputField.text != "";
        }
    }
}