using System;
using RTLTMPro;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    [RequireComponent(typeof(Button))]
    public class SubmittBattun : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputText;
        Button _myButton;

        private void Awake()
        {
            _myButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            inputText.onValueChanged.AddListener(OnInputChanged);
        }

        private void OnDisable()
        {
            inputText.onValueChanged.RemoveListener(OnInputChanged);
        }

        private void OnInputChanged(string newText)
        {
            _myButton.interactable = !string.IsNullOrEmpty(newText);
        }
    }
}