using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class TextForceProperties : MonoBehaviour
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