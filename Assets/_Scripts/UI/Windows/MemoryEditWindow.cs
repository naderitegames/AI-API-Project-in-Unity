using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class MemoryEditWindow : WindowPanel
    {
        [SerializeField] private MemoryManager _manager;
        MemoryContainer _targetMemory;
        [SerializeField] TMP_InputField descriptopnInputField;
        [SerializeField] TMP_InputField titleInputField;
        [SerializeField] Button targetSaveButton;
        [SerializeField] Button targetClearButon;
        [SerializeField] Button targetCancelButton;

        private void OnEnable()
        {
            targetCancelButton.onClick.AddListener(CancelButtonClick);
            targetSaveButton.onClick.AddListener(SaveEditedMemory);
            targetClearButon.onClick.AddListener(ClearDescriptionTotaly);
            descriptopnInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            titleInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
        }

        private void OnDisable()
        {
            targetCancelButton.onClick.RemoveListener(CancelButtonClick);
            targetSaveButton.onClick.RemoveListener(SaveEditedMemory);
            targetClearButon.onClick.RemoveListener(ClearDescriptionTotaly);

            descriptopnInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
            titleInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
        }

        private void CancelButtonClick()
        {
            SetActive(false);
        }

        private void ClearDescriptionTotaly()
        {
            descriptopnInputField.text = "";
        }

        public void EnableEditingForThisMemory(MemoryContainer targetMemory)
        {
            _targetMemory = targetMemory;
            SetUpInformationForEditing();
            SetActive(true);
        }

        void SetUpInformationForEditing()
        {
            descriptopnInputField.text = _targetMemory.Description;
            titleInputField.text = _targetMemory.Title;
        }

        void SaveEditedMemory()
        {
            UiManager.Instance.DisplayThisWarning(
                "آیا از ثبت تغییرات اطمینان دارید؟!",
                () =>
                {
                    ApplyUserChangesToTargetMemory();
                    _manager.UpdateThisEditedMemory(_targetMemory);
                    SetActive(false);
                },
                "خیر",
                "بله, ثبت کن");
        }
        void ApplyUserChangesToTargetMemory()
        {
            _targetMemory.UpdateTitle(titleInputField.text);
            _targetMemory.UpdateMemoryDescription(descriptopnInputField.text);
            _targetMemory.SetLastUpdateTime(DateTime.Now);
        }
        void OnOnInputFieldsChange(string arg0)
        {
            UpdateSaveButtonInteractable();
        }

        void UpdateSaveButtonInteractable()
        {
            targetSaveButton.interactable = descriptopnInputField.text != _targetMemory.Description ||
                                            titleInputField.text != _targetMemory.Title;
            targetClearButon.interactable = descriptopnInputField.text != "";
        }
    }
}