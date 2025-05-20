using System;
using System.Collections.Generic;
using System.Globalization;
using _Scripts.UI;
using RTLTMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ContextMenu = _Scripts.UI.Context_Menu.ContextMenu;

namespace _Scripts.Diary
{
    public class MemoryDisplay : MonoBehaviour
    {
        private MemoryManager _manager => MemoryManager.Instance;

        [SerializeField] Button targetOpeningButton;
        [SerializeField] Toggle targetSelectionToggle;
        [SerializeField] RTLTextMeshPro descriptionPlace;
        [SerializeField] RTLTextMeshPro titlePlace;
        [SerializeField] RTLTextMeshPro lastModifiedTimePlace;
        [SerializeField] ContextMenu contextMenu;

        [SerializeField] private Image targetPinnedIcon;
        //[SerializeField] private int charLimitationForDescription;
        //[SerializeField] private int charLimitationForTitle;

        public static List<MemoryDisplay> SelectedDisplayers = new List<MemoryDisplay>();
        public bool IsSelected => isSelected;

        private bool isSelected;
        private DiaryContainer _diary;

        static Action OnOtherContextMenuWillBeOpen;
        public static Action<List<MemoryDisplay>> OnSelectionsChanged;

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
            ChangeSelectionStateTo(false);
            contextMenu.SetActive(false);
            SelectedDisplayers.Clear();
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
            }
        }

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
                }
            }
            else
            {
                if (SelectedDisplayers.Contains(this))
                {
                    SelectedDisplayers.Remove(this);
                }
            }

            OnSelectionsChanged?.Invoke(SelectedDisplayers);
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