using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowPanel : MonoBehaviour
    {
        CanvasGroup _canvasGroup;
        [SerializeField] UnityEvent OnWindowActived;
        public CanvasGroup CanvasGroup => _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void SetActive(bool isActive)
        {
            if (isActive)
            {
                OnWindowActived?.Invoke();
            }

            _canvasGroup.alpha = isActive ? 1 : 0;
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }
    }
}