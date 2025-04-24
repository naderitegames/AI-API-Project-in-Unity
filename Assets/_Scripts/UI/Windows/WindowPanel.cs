using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowPanel : MonoBehaviour
    {
        [Tooltip("If it is empty,it will take its own canvas group component")]
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] UnityEvent OnWindowActived;
        public CanvasGroup CanvasGroup => canvasGroup;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void SetActive(bool isActive)
        {
            if (isActive)
            {
                OnWindowActived?.Invoke();
            }

            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }
    }
}