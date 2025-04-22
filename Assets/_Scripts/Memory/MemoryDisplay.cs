using System;
using System.Globalization;
using RTLTMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoryDisplay : MonoBehaviour
    {
        private MemoryManager _manager;
        [SerializeField] private int charLimitationForDescription;
        [SerializeField] private int charLimitationForTitle;
        [SerializeField] RTLTextMeshPro descriptionPlace;
        [SerializeField] RTLTextMeshPro titlePlace;
        [SerializeField] RTLTextMeshPro creationTimePlace;
        [SerializeField] RTLTextMeshPro lastModifiedTimePlace;
        [SerializeField] MemoryContextMenu memoryContextMenu;
        private MemoryContainer _memory;
        static Action OnOtherContextMenuWillBeOpen;

        private void OnEnable()
        {
            memoryContextMenu.CloseContextMenu();
            OnOtherContextMenuWillBeOpen += OnOnOtherContextMenuWillBeOpen;
            RefreshMemory();
        }

        private void OnDisable()
        {
            OnOtherContextMenuWillBeOpen -= OnOnOtherContextMenuWillBeOpen;
        }

        private void OnOnOtherContextMenuWillBeOpen()
        {
            memoryContextMenu.CloseContextMenu();
        }

        public void SetUp(MemoryContainer memory, MemoryManager manager)
        {
            this._manager = manager;
            UpdateMemory(memory);
        }

        public void UpdateMemory(MemoryContainer memory)
        {
            _memory = memory;
            RefreshMemory();
        }

        void RefreshMemory()
        {
            if (_memory != null)
            {
                titlePlace.text = GetShortPreview(_memory.Title, charLimitationForTitle);
                var targetDescription = _memory.Summary == "" ? _memory.Description : _memory.Summary;
                descriptionPlace.text = GetShortPreview(targetDescription, charLimitationForDescription);
                creationTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
                lastModifiedTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
            }
        }

        static string GetShortPreview(string text, int maxCharacters = 150)
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
        }

        public void OpenContextMenu()
        {
            OnOtherContextMenuWillBeOpen?.Invoke();
            memoryContextMenu.ToggleContextMenu();
        }

        public void DeleteThisMemory()
        {
            UiManager.Instance.DisplayThisWarning("از حذف اطمینان دارید؟!",
                () => { _manager.RemoveFromMemories(_memory); }, "خیر", "بله");
        }

        public void SummarizeThisMemory()
        {
            _manager.SummarizeThisMemoryThenSave(_memory);
        }

        public void TryEditingThisMemory()
        {
            UiManager.Instance.DisplayThisWarning("از ویرایش خاطره اطمینان دارید؟!",
                () => { _manager.OpenEditWindowForThisMemory(_memory); }, "خیر", "بله");
        }
    }
}