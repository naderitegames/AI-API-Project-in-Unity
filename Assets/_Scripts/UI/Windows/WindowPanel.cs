using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowPanel : MonoBehaviour
    {
        CanvasGroup _canvasGroup;
        [SerializeField] UnityEvent OnWindowActived;
        public CanvasGroup CanvasGroup => _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void SetActive(bool t)
        {
            if (t)
            {
                OnWindowActived?.Invoke();
            }

            _canvasGroup.alpha = t ? 1 : 0;
            _canvasGroup.interactable = t;
            _canvasGroup.blocksRaycasts = t;
        }
    }
}