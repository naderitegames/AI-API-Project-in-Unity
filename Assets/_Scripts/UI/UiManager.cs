using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public enum WindowType
    {
        Add = 0,
        Home,
        Chat
    }

    public class UiManager : MonoBehaviour
    {
        [SerializeField] AddMemoryWindow addWindowPanel;
        [SerializeField] HomeWindow homeWindowPanel;
        [SerializeField] ChatWindow chatWindowPanel;
        [SerializeField] private WarningWindow _warningWindow;
        Dictionary<WindowType, WindowPanel> _allPanels = new Dictionary<WindowType, WindowPanel>();
        WindowPanel _activePanel;

        public static UiManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _allPanels.Add(WindowType.Add, addWindowPanel);
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
                ActivatePanel((int)var.Key);
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

        public void DisplayThisWarning(string message, string okText = " بسیار خب!")
        {
            _warningWindow.UpdateTextAndDisplay(message, okText);
        }
    }
}