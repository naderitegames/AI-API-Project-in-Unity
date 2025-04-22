using System;
using UnityEngine;

namespace _Scripts
{
    public class WarningManager : MonoBehaviour
    {
        [SerializeField] WarningWindow warningWindow;
        public void Awake()
        {
            warningWindow.SetActive(false);
        }
    }
}