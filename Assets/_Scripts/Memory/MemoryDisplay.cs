using System;
using System.Collections.Generic;
using System.Globalization;
using RTLTMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class MemoryDisplay : MonoBehaviour
    {
        private MemoryManager _manager => MemoryManager.Instance;
        [SerializeField] Button targetOpeningButton;
        [SerializeField] Toggle targetSelectionToggle;

        [SerializeField] private Image targetPinnedIcon;

        //[SerializeField] private int charLimitationForDescription;
        //[SerializeField] private int charLimitationForTitle;
        [SerializeField] RTLTextMeshPro descriptionPlace;
        [SerializeField] RTLTextMeshPro titlePlace;
        [SerializeField] RTLTextMeshPro lastModifiedTimePlace;
        public static List<MemoryDisplay> SelectedDisplayers = new List<MemoryDisplay>();

        [FormerlySerializedAs("memoryContextMenu")] [SerializeField]
        ContextMenu contextMenu;

        private DiaryContainer _diary;
        static Action OnOtherContextMenuWillBeOpen;
        public static Action<List<MemoryDisplay>> OnSelectionsChanged;
        public bool IsSelected => isSelected;
        private bool isSelected;

        private void OnEnable()
        {
            contextMenu.SetActive(false);
            OnOtherContextMenuWillBeOpen += OnOnOtherContextMenuWillBeOpen;
            targetOpeningButton.onClick.AddListener(TryOpeningThisMemory);
            OnSelectionsChanged += OnOnSelectionsChanged;
            targetSelectionToggle.onValueChanged.AddListener(ChangeSelectionStateTo);
            RefreshMemory();
            isSelected = false;
            targetSelectionToggle.isOn = false;
            targetSelectionToggle.gameObject.SetActive(false);
            contextMenu.SetActive(false);
        }

        private void OnDisable()
        {
            OnOtherContextMenuWillBeOpen -= OnOnOtherContextMenuWillBeOpen;
            targetOpeningButton.onClick.RemoveListener(TryOpeningThisMemory);
            OnSelectionsChanged -= OnOnSelectionsChanged;
            targetSelectionToggle.onValueChanged.RemoveListener(ChangeSelectionStateTo);
        }

        private void OnOnOtherContextMenuWillBeOpen()
        {
            contextMenu.SetActive(false);
        }

        public void SetUp(DiaryContainer diary)
        {
            UpdateMemory(diary);
        }

        public void UpdateMemory(DiaryContainer diary)
        {
            _diary = diary;
            targetPinnedIcon.gameObject.SetActive(_diary.IsPinned);
            RefreshMemory();
        }

        void RefreshMemory()
        {
            if (_diary != null)
            {
                titlePlace.text = _diary.Title;
                var targetDescription = _diary.Summary == "" ? _diary.Description : _diary.Summary;
                descriptionPlace.text = targetDescription;
                lastModifiedTimePlace.text = _diary.LastUpdateTime.ToString(CultureInfo.InvariantCulture);

                /*titlePlace.text = GetShortPreview(_memory.Title, charLimitationForTitle);
                var targetDescription = _memory.Summary == "" ? _memory.Description : _memory.Summary;
                descriptionPlace.text = GetShortPreview(targetDescription, charLimitationForDescription);
                creationTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
                lastModifiedTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);*/
            }
        }

        /*static string GetShortPreview(string text, int maxCharacters = 150)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            if (text.Length <= maxCharacters)
                return text;

            // برای اینکه کلمات نصفه نشکنن
            string shortened = text.Substring(0, maxCharacters);
            int lastSpace = shortened.LastIndexOf(' ');

            if (lastSpace > 0)
                shortened = shortened.Substring(0, lastSpace);

            return shortened + " ...";
        }*/

        public void OpenContextMenu()
        {
            OnOtherContextMenuWillBeOpen?.Invoke();
            contextMenu.SetActive(true);
        }

        public void DeleteThisMemory()
        {
            UiManager.Instance.DisplayThisWarning("از حذف اطمینان دارید؟!",
                () => { _manager.RemoveFromMemories(_diary); }, "خیر", "بله");
        }

        public void TogglePinState()
        {
            _manager.ToggleThisMemoryPinState(_diary);
        }

        public void SummarizeThisMemory()
        {
            _manager.SummarizeThisMemoryThenSave(_diary);
        }

        public void TryEditingThisMemory()
        {
            UiManager.Instance.DisplayThisWarning("از ویرایش خاطره اطمینان دارید؟!",
                () => { UiManager.Instance.OpenNewDiaryEditWindow(_diary); }, "خیر", "بله");
        }

        public void TryOpeningThisMemory()
        {
            UiManager.Instance.OpenNewDiaryEditWindow(_diary);
        }

        public void ChangeSelectionStateTo()
        {
            ChangeSelectionStateTo(!isSelected);
        }

        public void ChangeSelectionStateTo(bool b)
        {
            isSelected = b;
            if (isSelected)
            {
                // Just a double check for make sure not dealing with null ...
                if (!SelectedDisplayers.Contains(this))
                {
                    SelectedDisplayers.Add(this);
                    contextMenu.SetActive(false);
                    OnSelectionsChanged?.Invoke(SelectedDisplayers);
                }
            }
            else
            {
                if (SelectedDisplayers.Contains(this))
                {
                    SelectedDisplayers.Remove(this);
                    OnSelectionsChanged?.Invoke(SelectedDisplayers);
                }
            }
        }

        private void OnOnSelectionsChanged(List<MemoryDisplay> selecteds)
        {
            if (selecteds.Count >= 1)
            {
                targetSelectionToggle.gameObject.SetActive(true);
                targetSelectionToggle.isOn = isSelected;
            }
            else
            {
                targetSelectionToggle.gameObject.SetActive(false);
            }
        }

        public void ChatWithAI()
        {
            isSelected = true;
            SelectedDisplayers.Add(this);
            UiManager.Instance.DisplayChatForSelectedDiaries();
        }

        public DiaryContainer GetDiary()
        {
            return _diary;
        }

        public static void ClearSelectedDiaries()
        {
            SelectedDisplayers.Clear();
        }
    }
}