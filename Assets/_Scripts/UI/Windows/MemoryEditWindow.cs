using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Chat;
using _Scripts.Diary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Windows
{
    public class MemoryEditWindow : WindowPanel
    {
        DiaryContainer _targetDiary;
        [SerializeField] ChatWithAiManager _aiManager;
        [SerializeField] TMP_InputField descriptopnInputField;
        [SerializeField] TMP_InputField summaryInputField;
        [SerializeField] TMP_InputField titleInputField;
        [SerializeField] Button targetSaveButton;
        [SerializeField] Button targetClearButon;
        [SerializeField] Button targetChatButton;
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
            targetChatButton.onClick.AddListener(StartChatWithAI);
            descriptopnInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            summaryInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            titleInputField.onValueChanged.AddListener(OnOnInputFieldsChange);
            turnOnOriginalDiscriptionButton.onClick.AddListener(OnDisplayOriginalDescription);
            targetSummarizelButton.onClick.AddListener(SummarizeDescriptionNow);
            turnOnDiscriptionSummaryButton.onClick.AddListener(OnDisplaylDescriptionSummary);

            SwitchOriginalDescriptionStateTo(true);
        }

        private void OnDisable()
        {
            targetCancelButton.onClick.RemoveListener(CancelButtonClick);
            targetSaveButton.onClick.RemoveListener(SaveEditedMemory);
            targetClearButon.onClick.RemoveListener(ClearDescriptionTotally);
            targetChatButton.onClick.RemoveListener(StartChatWithAI);
            turnOnOriginalDiscriptionButton.onClick.RemoveListener(OnDisplayOriginalDescription);
            turnOnDiscriptionSummaryButton.onClick.RemoveListener(OnDisplaylDescriptionSummary);
            descriptopnInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
            summaryInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
            targetSummarizelButton.onClick.RemoveListener(SummarizeDescriptionNow);
            titleInputField.onValueChanged.RemoveListener(OnOnInputFieldsChange);
        }

        private void StartChatWithAI()
        {
            List<DiaryContainer> targets = new List<DiaryContainer> { _targetDiary };
            _aiManager.PrepareForChat(targets);
            UiManager.Instance.ActivatePanel(WindowType.Chat);
        }

        private void SummarizeDescriptionNow()
        {
            _targetDiary.UpdateDescription(descriptopnInputField.text);
            MemoryManager.Instance.SummarizeThisMemoryThenSave(_targetDiary, (t) =>
            {
                _targetDiary = t;
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

        public void SetUpForWritingDiary(DiaryContainer targetDiary)
        {
            _targetDiary = targetDiary;
            SetUpInformationForEditing();
        }

        public void SetUpForWritingDiary()
        {
            _targetDiary = new DiaryContainer();
            SetUpInformationForEditing();
        }

        void SetUpInformationForEditing()
        {
            descriptopnInputField.text = _targetDiary.Description;
            titleInputField.text = _targetDiary.Title;
            summaryInputField.text = _targetDiary.Summary;
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
                    MemoryManager.Instance.UpdateThisEditedMemoryIfExists(_targetDiary);
                    UiManager.Instance.DisplayHome();
                },
                "خیر",
                "بله, ثبت کن");
        }

        void ApplyUserChangesToTargetMemory()
        {
            _targetDiary.UpdateTitle(titleInputField.text);
            _targetDiary.UpdateDescription(descriptopnInputField.text);
            _targetDiary.UpdateSummary(summaryInputField.text);
        }

        void OnOnInputFieldsChange(string arg0)
        {
            UpdateSaveButtonInteractable();
        }

        void UpdateSaveButtonInteractable()
        {
            if (descriptopnInputField.text != _targetDiary.Description)
                _targetDiary.UpdateSummary("");
            targetSaveButton.interactable = descriptopnInputField.text != _targetDiary.Description ||
                                            titleInputField.text != _targetDiary.Title ||
                                            summaryInputField.text != _targetDiary.Summary;
            targetClearButon.interactable = descriptopnInputField.text != "";
        }
    }
}