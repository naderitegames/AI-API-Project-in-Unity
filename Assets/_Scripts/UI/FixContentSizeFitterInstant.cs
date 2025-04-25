using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class FixContentSizeFitterInstant : MonoBehaviour
    {
        public TMP_InputField inputField;
        public RectTransform targetRect;

        void Start()
        {
            inputField.onValueChanged.AddListener(delegate { RefreshFitter(); });
        }

        void RefreshFitter()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(targetRect);
        }
    }
}