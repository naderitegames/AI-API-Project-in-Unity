using System;
using System.Collections.Generic;
using System.Globalization;
using RTLTMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class MemoryDisplay : MonoBehaviour
    {
        private MemoryManager _manager;
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

        private MemoryContainer _memory;
        static Action OnOtherContextMenuWillBeOpen;
        private static Action OnSelectionsChanged;
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

        public void SetUp(MemoryContainer memory, MemoryManager manager)
        {
            this._manager = manager;
            UpdateMemory(memory);
        }

        public void UpdateMemory(MemoryContainer memory)
        {
            _memory = memory;
            targetPinnedIcon.gameObject.SetActive(_memory.IsPinned);
            RefreshMemory();
        }

        void RefreshMemory()
        {
            if (_memory != null)
            {
                titlePlace.text = _memory.Title;
                var targetDescription = _memory.Summary == "" ? _memory.Description : _memory.Summary;
                descriptionPlace.text = targetDescription;
                lastModifiedTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);

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
                () => { _manager.RemoveFromMemories(_memory); }, "خیر", "بله");
        }

        public void TogglePinState()
        {
            _manager.ToggleThisMemoryPinState(_memory);
        }

        public void SummarizeThisMemory()
        {
            _manager.SummarizeThisMemoryThenSave(_memory);
        }

        public void TryEditingThisMemory()
        {
            UiManager.Instance.DisplayThisWarning("از ویرایش خاطره اطمینان دارید؟!",
                () => { UiManager.Instance.OpenNewDiaryEditWindow(_memory); }, "خیر", "بله");
        }

        public void TryOpeningThisMemory()
        {
            UiManager.Instance.OpenNewDiaryEditWindow(_memory);
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
                    OnSelectionsChanged?.Invoke();
                }
            }
            else
            {
                if (SelectedDisplayers.Contains(this))
                {
                    SelectedDisplayers.Remove(this);
                    OnSelectionsChanged?.Invoke();
                }
            }
        }

        private void OnOnSelectionsChanged()
        {
            if (SelectedDisplayers.Count >= 1)
            {
                targetSelectionToggle.gameObject.SetActive(true);
                targetSelectionToggle.isOn = isSelected;
            }
            else
            {
                targetSelectionToggle.gameObject.SetActive(false);
            }
        }
    }
}