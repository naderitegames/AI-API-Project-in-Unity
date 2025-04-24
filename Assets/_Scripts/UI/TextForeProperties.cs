using System;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class TextForeProperties : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI targetText;
        [SerializeField] bool wrappingEnabled;

        private void Update()
        {
            if (targetText.enableWordWrapping != wrappingEnabled)
            {
                targetText.enableWordWrapping = wrappingEnabled;
            }
        }
    }
}