using System;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MemoryContextMenu : MonoBehaviour
    {
        [SerializeField]CanvasGroup _canvasGroup;
        bool _isOpen = false;

        private void Awake()
        {
            SetActive(false);
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
    }
}