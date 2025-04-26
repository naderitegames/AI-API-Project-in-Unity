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
        [SerializeField] HomeWindow homeWindowPanel;
        [SerializeField] ChatWindow chatWindowPanel;
        [SerializeField] private WarningWindow _warningWindow;
        Dictionary<WindowType, WindowPanel> _allPanels = new Dictionary<WindowType, WindowPanel>();
        WindowPanel _activePanel;

        public static UiManager Instance { get; private set; }

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

        public void OpenNewDiaryEditWindow(MemoryContainer c)
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
    }
}