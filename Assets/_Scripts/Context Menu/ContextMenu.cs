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
        /*[SerializeField] MemoryDisplay _memoryDisplay;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] Button targetEditButton;
        [SerializeField] Button targetSummarizeButton;
        [SerializeField] Button targetDeleteButton;
        [SerializeField] Button targetPinButton;
        bool _isOpen = false;

        private void Awake()
        {
            SetActive(false);
        }

        private void OnEnable()
        {
            targetEditButton.onClick.AddListener(_memoryDisplay.TryEditingThisMemory);
            targetSummarizeButton.onClick.AddListener(_memoryDisplay.SummarizeThisMemory);
            targetDeleteButton.onClick.AddListener(_memoryDisplay.DeleteThisMemory);
            targetPinButton.onClick.AddListener(_memoryDisplay.DeleteThisMemory);
        }

        private void OnDisable()
        {
            targetEditButton.onClick.RemoveListener(_memoryDisplay.TryEditingThisMemory);
            targetSummarizeButton.onClick.RemoveListener(_memoryDisplay.SummarizeThisMemory);
            targetDeleteButton.onClick.RemoveListener(_memoryDisplay.DeleteThisMemory);
        }

        void SetActive(bool b)
        {
            _canvasGroup.interactable = b;
            _canvasGroup.blocksRaycasts = b;
            _canvasGroup.alpha = b ? 1 : 0;
        }

        public void ToggleContextMenu()
        {
            _isOpen = !_isOpen;
            SetActive(_isOpen);
        }

        public void CloseContextMenu()
        {
            _isOpen = false;
            SetActive(_isOpen);
        }*/
    }
}