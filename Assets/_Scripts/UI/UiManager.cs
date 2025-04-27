using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public enum WindowType
    {
        //Add = 0,
        Home = 0,
        Chat,
        Edit
    }

    public class UiManager : MonoBehaviour
    {
        //[SerializeField] AddMemoryWindow addWindowPanel;
        [SerializeField] MemoryEditWindow memoryEditWindow;
        [SerializeField] private ChatWithAiManager chatWithAiManager;
        [SerializeField] HomeWindow homeWindowPanel;
        [SerializeField] ChatWindow chatWindowPanel;
        [SerializeField] private WarningWindow _warningWindow;
        Dictionary<WindowType, WindowPanel> _allPanels = new Dictionary<WindowType, WindowPanel>();
        WindowPanel _activePanel;
        [SerializeField] ContextMenu selectedDiariesContextMenu;

        public static UiManager Instance { get; private set; }

        private void OnEnable()
        {
            selectedDiariesContextMenu.gameObject.SetActive(false);
            MemoryDisplay.OnSelectionsChanged += OnSelectionStateChanged;
        }

        private void OnDisable()
        {
            MemoryDisplay.OnSelectionsChanged -= OnSelectionStateChanged;
        }

        private void OnSelectionStateChanged(List<MemoryDisplay> selectedDiaries)
        {
            selectedDiariesContextMenu.gameObject.SetActive(selectedDiaries.Count > 0);
        }

        private void Awake()
        {
            Instance = this;
            //_allPanels.Add(WindowType.Add, addWindowPanel);
            _allPanels.Add(WindowType.Edit, memoryEditWindow);
            _allPanels.Add(WindowType.Home, homeWindowPanel);
            _allPanels.Add(WindowType.Chat, chatWindowPanel);
            _activePanel = _allPanels[WindowType.Home];
        }

        private void Start()
        {
            DisAppearAllPanels();
            ActivatePanel(WindowType.Home);
        }

        void DisAppearAllPanels()
        {
            foreach (var var in _allPanels)
            {
                _allPanels[var.Key].SetActive(false);
            }
        }

        public void ActivatePanel(int typeIndex)
        {
            ActivatePanel((WindowType)typeIndex);
        }

        public void ActivatePanel(WindowType type)
        {
            //todo: use dotween here
            _activePanel.SetActive(false);
            _activePanel = _allPanels[type];
            _activePanel.SetActive(true);
        }

        public void DisplayThisWarning(string warningText, Action job, string cancelText = "Cancel",
            string okText = "Continue")
        {
            _warningWindow.UpdateTextAndDisplay(warningText, job, cancelText, okText);
        }

        public void DisplayThisWarning(string message, string okText)
        {
            _warningWindow.UpdateTextAndDisplay(message, okText);
        }

        public void OpenNewDiaryEditWindow()
        {
            ActivatePanel(WindowType.Edit);
            memoryEditWindow.SetUpForWritingDiary();
        }

        public void OpenNewDiaryEditWindow(DiaryContainer c)
        {
            ActivatePanel(WindowType.Edit);
            memoryEditWindow.SetUpForWritingDiary(c);
        }

        public void DisplayHome()
        {
            ActivatePanel(WindowType.Home);
        }

        public void DisplayThisWarning(string description)
        {
            _warningWindow.UpdateTextAndDisplay(description);
        }

        public void DisplayChatForSelectedDiaries()
        {
            ActivatePanel(WindowType.Chat);
            List<DiaryContainer> targetDiaries = new List<DiaryContainer>();
            foreach (var target in MemoryDisplay.SelectedDisplayers)
            {
                targetDiaries.Add(target.GetDiary());
            }

            chatWithAiManager.PrepareForChat(targetDiaries);
        }
    }
}