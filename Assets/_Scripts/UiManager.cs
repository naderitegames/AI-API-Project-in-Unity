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
        [SerializeField] WindowPanel addWindowPanel;
        [SerializeField] WindowPanel homeWindowPanel;
        [SerializeField] WindowPanel chatWindowPanel;
        Dictionary<WindowType, WindowPanel> _allPanels = new Dictionary<WindowType, WindowPanel>();
        WindowPanel _activePanel;

        private void Awake()
        {
            _allPanels.Add(WindowType.Add, addWindowPanel);
            _allPanels.Add(WindowType.Home, homeWindowPanel);
            _allPanels.Add(WindowType.Chat, chatWindowPanel);
            _activePanel = _allPanels[WindowType.Home];
        }

        private void Start()
        {
            DisAppearAllPanels();
            ActivatePanel((int)WindowType.Home);
        }

        void DisAppearAllPanels()
        {
            foreach (var var in _allPanels)
            {
                ActivatePanel((int)var.Key);
            }
        }
        void ActivatePanel(int typeIndex)
        {
            //todo: use dotween here
            _activePanel.SetActive(false);
            _activePanel = _allPanels[(WindowType)typeIndex];
            _activePanel.SetActive(true);
        }

        public void DisplaySearchResults(List<MemoryContainer> results)
        {
            if (results != null)
            {
                
            }
            else
            {
                
            }
        }
    }
}