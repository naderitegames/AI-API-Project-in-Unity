using _Scripts.UI.Windows;
using UnityEngine;

namespace _Scripts.UI
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