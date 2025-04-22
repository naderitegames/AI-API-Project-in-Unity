using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MemoryContextMenu : MonoBehaviour
    {
        [SerializeField] MemoryDisplay _memoryDisplay;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] Button targetEditButton;
        [SerializeField] Button targetSummarizeButton;
        [SerializeField] Button targetDeleteButton;
        bool _isOpen = false;

        private void Awake()
        {
            SetActive(false);
        }

        private void OnEnable()
        {
            targetEditButton.onClick.AddListener(EditThisMessage);
            targetSummarizeButton.onClick.AddListener(SummarizeThisMessage);
            targetDeleteButton.onClick.AddListener(DeleteThisMessage);
        }

        private void OnDisable()
        {
            targetEditButton.onClick.RemoveListener(EditThisMessage);
            targetSummarizeButton.onClick.RemoveListener(SummarizeThisMessage);
            targetDeleteButton.onClick.RemoveListener(DeleteThisMessage);
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
        }

        void DeleteThisMessage()
        {
            _memoryDisplay.DeleteThisMemory();
        }

        void EditThisMessage()
        {
            _memoryDisplay.TryEditingThisMemory();
        }

        void SummarizeThisMessage()
        {
            _memoryDisplay.SummarizeThisMemory();
        }
    }
}