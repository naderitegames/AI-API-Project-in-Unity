using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.AI.Embedding;
using _Scripts.Diary;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Search_Box
{
    public enum SearchType
    {
        KeyWords,
        AIMode
    }

    public class SearchBox : MonoBehaviour
    {
        [SerializeField] Button searchButton;
        [SerializeField] Button clearButton;
        [SerializeField] Toggle aiModeSearch;
        [SerializeField] TMP_InputField searchInputField;
        [SerializeField] MemoryManager memoryManager;

        private void OnEnable()
        {
            clearButton.onClick.AddListener(ClearAndFocusOnInputField);
            searchButton.onClick.AddListener(OnSearchButtonClicked);
            searchInputField.onValueChanged.AddListener(OnSearchBoxValueChanged);
            RefreshSearchButtonIcon();
        }

        private void OnDisable()
        {
            clearButton.onClick.RemoveListener(ClearAndFocusOnInputField);
            searchButton.onClick.RemoveListener(OnSearchButtonClicked);
            searchInputField.onValueChanged.RemoveListener(OnSearchBoxValueChanged);
        }

        private void Awake()
        {
            aiModeSearch.isOn = false;
        }

        private async void OnSearchButtonClicked()
        {
            if (string.IsNullOrWhiteSpace(searchInputField.text))
                return;


            if (aiModeSearch.isOn)
            {
                SearchInMemories(searchInputField.text, SearchType.AIMode);
            }
            else
            {
                SearchInMemories(searchInputField.text, SearchType.KeyWords);
            }

            RefreshSearchButtonIcon();
        }

        public async void SearchInMemories(string targetText, SearchType searchType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetText))
                {
                    memoryManager.RefreshMemoriesList();
                    return;
                }

                var allMemories = MemoryManager.Instance.AllMemories;
                List<DiaryContainer> results;
                switch (searchType)
                {
                    case SearchType.AIMode:
                        results = await MemorySearch.SearchMemoriesByEmbedding(targetText, allMemories);
                        break;
                    default:
                    case SearchType.KeyWords:
                        results = MemorySearch.SearchMemoriesByKeyword(targetText, allMemories);
                        break;
                }

                memoryManager.DisplayThisMemories(results);
            }
            catch (Exception e)
            {
                Debug.LogError("error :" + e);
            }
        }

        private void ClearAndFocusOnInputField()
        {
            searchInputField.Select();
            searchInputField.text = "";
            RefreshSearchButtonIcon();
        }

        private void OnSearchBoxValueChanged(string targetText)
        {
            if (!aiModeSearch.isOn || (aiModeSearch.isOn && string.IsNullOrWhiteSpace(targetText)))
            {
                SearchInMemories(targetText, SearchType.KeyWords);
            }

            RefreshSearchButtonIcon();
        }

        void RefreshSearchButtonIcon()
        {
            bool isInputFieldEmpty = string.IsNullOrWhiteSpace(searchInputField.text);
            clearButton.gameObject.SetActive(!isInputFieldEmpty);
        }
    }
}