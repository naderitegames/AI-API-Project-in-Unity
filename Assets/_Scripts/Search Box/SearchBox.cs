using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class SearchBox : MonoBehaviour
    {
        Button searchButton;
        [SerializeField] TMP_InputField searchInputField;
        [SerializeField] MemoryManager memoryManager;

        private void OnEnable()
        {
            searchInputField.onValueChanged.AddListener(OnSearchBoxValueChanged);
        }

        private void OnDisable()
        {
            searchInputField.onValueChanged.RemoveListener(OnSearchBoxValueChanged);
        }

        private void OnSearchBoxValueChanged(string arg0)
        {
            //todo: if it is not on AI Mode ...
            memoryManager.SearchInMemories(searchInputField);
        }
    }
}