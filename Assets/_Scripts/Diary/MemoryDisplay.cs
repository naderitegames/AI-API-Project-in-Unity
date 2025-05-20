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
        public static Action<List<MemoryDisplay>> OnSelectionsChanged;
        static Action _onOtherContextMenuWillBeOpen;
        public static List<MemoryDisplay> SelectedDisplayers = new List<MemoryDisplay>();
        private MemoryManager _manager => MemoryManager.Instance;
        public DiaryContainer Diary => _diary;

        [SerializeField] Button targetOpeningButton;
        [SerializeField] Toggle targetSelectionToggle;
        [SerializeField] RTLTextMeshPro descriptionPlace;
        [SerializeField] RTLTextMeshPro titlePlace;
        [SerializeField] RTLTextMeshPro lastModifiedTimePlace;
        [SerializeField] ContextMenu contextMenu;
        [SerializeField] Image targetPinnedIcon;

        private bool _isSelected;
        private DiaryContainer _diary;

        private void OnEnable()
        {
            contextMenu.SetActive(false);
            _onOtherContextMenuWillBeOpen += OnOnOtherContextMenuWillBeOpen;
            targetOpeningButton.onClick.AddListener(TryOpeningThisMemory);
            OnSelectionsChanged += OnOnSelectionsChanged;
            targetSelectionToggle.onValueChanged.AddListener(ChangeSelectionStateTo);
            RefreshMemory();
            _isSelected = false;
            targetSelectionToggle.isOn = false;
            targetSelectionToggle.gameObject.SetActive(false);
            contextMenu.SetActive(false);
        }

        private void OnDisable()
        {
            _onOtherContextMenuWillBeOpen -= OnOnOtherContextMenuWillBeOpen;
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

        // use with buttons click event in inspector ...
        public void OpenContextMenu()
        {
            _onOtherContextMenuWillBeOpen?.Invoke();
            contextMenu.SetActive(true);
        }

        public void DeleteThisMemory()
        {
            UiManager.Instance.DisplayThisWarning("از حذف اطمینان دارید؟!",
                () => { _manager.RemoveFromAllDiaries(_diary); }, "خیر", "بله");
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
            UiManager.Instance.OpenNewDiaryEditWindow(_diary);
            //UiManager.Instance.DisplayThisWarning("از ویرایش خاطره اطمینان دارید؟!",
            //    () => { UiManager.Instance.OpenNewDiaryEditWindow(_diary); }, "خیر", "بله");
        }

        private void TryOpeningThisMemory()
        {
            UiManager.Instance.OpenNewDiaryEditWindow(_diary);
        }

        public void ToggleSelectionState()
        {
            ChangeSelectionStateTo(!_isSelected);
        }

        private void ChangeSelectionStateTo(bool b)
        {
            _isSelected = b;
            if (_isSelected)
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

        private void OnOnSelectionsChanged(List<MemoryDisplay> selected)
        {
            // Display selection state Toggle or not
            if (selected.Count >= 1)
            {
                targetSelectionToggle.gameObject.SetActive(true);
                targetSelectionToggle.isOn = _isSelected;
            }
            else
            {
                targetSelectionToggle.gameObject.SetActive(false);
            }
        }

        public void ChatWithAI()
        {
            _isSelected = true;
            SelectedDisplayers.Add(this);
            UiManager.Instance.DisplayChatForSelectedDiaries();
        }
    }
}