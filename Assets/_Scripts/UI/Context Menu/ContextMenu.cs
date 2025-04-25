using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ContextMenu : MonoBehaviour
    {
        static WindowPanel _activeContextMenu;
        [SerializeField] private WindowPanel targetWindowPanel;
        [SerializeField] private Button targetOpeningButton;
        [SerializeField] private Button targetClosingButton;

        private void OnEnable()
        {
            targetOpeningButton.onClick.AddListener(OnOpeningButtonClicked);
            targetClosingButton.onClick.AddListener(OnClosingButtonClicked);
        }

        private void OnDisable()
        {
            targetOpeningButton.onClick.RemoveListener(OnOpeningButtonClicked);
            targetClosingButton.onClick.RemoveListener(OnClosingButtonClicked);
        }

        private void OnClosingButtonClicked()
        {
            SetActive(false);
        }

        private void OnOpeningButtonClicked()
        {
            SetActive(true);
        }

        public void SetActive(bool isActive)
        {
            targetWindowPanel.SetActive(isActive);
            _activeContextMenu?.SetActive(isActive);
            _activeContextMenu = isActive ? targetWindowPanel : null;
        }
    }
}