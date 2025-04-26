using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class MemoryEditWindow : WindowPanel
    {
        MemoryContainer _targetMemory;
        [SerializeField] TMP_InputField descriptopnInputField;
        [SerializeField] TMP_InputField summaryInputField;
        [SerializeField] TMP_InputField titleInputField;
        [SerializeField] Button targetSaveButton;
        [SerializeField] Button targetClearButon;
        [SerializeField] Button targetCancelButton;
        [SerializeField] Button targetSummarizelButton;
        [SerializeField] Button turnOnOriginalDiscriptionButton;
        [SerializeField] Button turnOnDiscriptionSummaryButton;
        bool _isDisplayingOriginalText = false;

        private void OnEnable()
        {
            targetCancelButton.onClick.AddListener(CancelButtonClick);
            targetSaveButton.onClick.AddListener(SaveEditedMemory);
            targetClearButon.onClick.AddListener(ClearDescriptionTotally);
            descriptopnInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            summaryInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            titleInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            turnOnOriginalDiscriptionButton.onClick.AddListener(OnDisplayOriginalDescription);
            targetSummarizelButton.onClick.AddListener(SummurizeDescriptionNow);
            turnOnDiscriptionSummaryButton.onClick.AddListener(OnDisplaylDescriptionSummary);

            SwitchOriginalDescriptionStateTo(true);
        }

        private void OnDisable()
        {
            targetCancelButton.onClick.RemoveListener(CancelButtonClick);
            targetSaveButton.onClick.RemoveListener(SaveEditedMemory);
            targetClearButon.onClick.RemoveListener(ClearDescriptionTotally);
            turnOnOriginalDiscriptionButton.onClick.RemoveListener(OnDisplayOriginalDescription);
            turnOnDiscriptionSummaryButton.onClick.RemoveListener(OnDisplaylDescriptionSummary);
            descriptopnInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
            summaryInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
            targetSummarizelButton.onClick.RemoveListener(SummurizeDescriptionNow);
            titleInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
        }

        private void SummurizeDescriptionNow()
        {
            _targetMemory.UpdateMemoryDescription(descriptopnInputField.text);
            MemoryManager.Instance.SummarizeThisMemoryThenSave(_targetMemory, (t) =>
            {
                _targetMemory = t;
                SetUpInformationForEditing();
            });
        }

        private void OnDisplaylDescriptionSummary()
        {
            SwitchOriginalDescriptionStateTo(false);
        }

        private void OnDisplayOriginalDescription()
        {
            SwitchOriginalDescriptionStateTo(true);
        }

        void SwitchOriginalDescriptionStateTo(bool b)
        {
            turnOnDiscriptionSummaryButton.interactable = b;
            turnOnOriginalDiscriptionButton.interactable = !b;
            _isDisplayingOriginalText = b;
            descriptopnInputField.gameObject.SetActive(_isDisplayingOriginalText);
            summaryInputField.gameObject.SetActive(!_isDisplayingOriginalText);
        }

        private void CancelButtonClick()
        {
            UiManager.Instance.DisplayHome();
        }

        private void ClearDescriptionTotally()
        {
            descriptopnInputField.text = "";
            titleInputField.text = "";
            summaryInputField.text = "";
        }

        public void SetUpForWritingDiary(MemoryContainer targetMemory)
        {
            _targetMemory = targetMemory;
            SetUpInformationForEditing();
        }

        public void SetUpForWritingDiary()
        {
            _targetMemory = new MemoryContainer();
            SetUpInformationForEditing();
        }

        void SetUpInformationForEditing()
        {
            descriptopnInputField.text = _targetMemory.Description;
            titleInputField.text = _targetMemory.Title;
            summaryInputField.text = _targetMemory.Summary;
        }

        void SaveEditedMemory()
        {
            string messageText = string.IsNullOrEmpty(descriptopnInputField.text)
                ? "متن خاطره خالی است. آیا از ذخیره اطمینان دارید؟!"
                : "آیا از ثبت تغییرات اطمینان دارید؟!";
            UiManager.Instance.DisplayThisWarning(
                messageText,
                () =>
                {
                    ApplyUserChangesToTargetMemory();
                    MemoryManager.Instance.UpdateThisEditedMemoryIfExists(_targetMemory);
                    UiManager.Instance.DisplayHome();
                },
                "خیر",
                "بله, ثبت کن");
        }

        void ApplyUserChangesToTargetMemory()
        {
            _targetMemory.UpdateTitle(titleInputField.text);
            _targetMemory.UpdateMemoryDescription(descriptopnInputField.text);
            _targetMemory.UpdateSummary(summaryInputField.text);
            _targetMemory.SetLastUpdateTime(DateTime.Now);
        }

        void OnOnInputFieldsChange(string arg0)
        {
            UpdateSaveButtonInteractable();
        }

        void UpdateSaveButtonInteractable()
        {
            if (descriptopnInputField.text != _targetMemory.Description)
                _targetMemory.UpdateSummary("");
            targetSaveButton.interactable = descriptopnInputField.text != _targetMemory.Description ||
                                            titleInputField.text != _targetMemory.Title ||
                                            summaryInputField.text != _targetMemory.Summary;
            targetClearButon.interactable = descriptopnInputField.text != "";
        }
    }
}