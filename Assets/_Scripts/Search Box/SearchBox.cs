using _Scripts.AI.Embedding;
using _Scripts.Diary;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Search_Box
{
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

            var allMemories = memoryManager.GetAllMemories();

            if (aiModeSearch.isOn)
            {
                UiManager.Instance.DisplayThisWarning("در حال برسی اطلاعات. کمی صبر کنید");
                // بررسی اینکه آیا هر Diary دارای embedding است یا نه
                foreach (var diary in allMemories)
                {
                    //اگر حاوی embedding نبود
                    if (!diary.HasEmbedding())
                    {
                        // دریافت و ذخیره embedding برای این diary
                        await EmbeddingManager.Instance.SaveEmbeddingForDiaryAsync(diary);
                    }
                    print(diary.Embedding);
                }

                UiManager.Instance.DisplayThisWarning("در حال ارسال اطلاعات. کمی صبر کنید");

                // حالا جستجو با AI رو انجام میدیم
                var results =
                    await EmbeddingManager.Instance.SearchSimilarDiariesAsync(searchInputField.text, allMemories);
                UiManager.Instance.CloseWarningWindow();
                memoryManager.DisplaySearchResults(results);
            }
            else
            {
                //جست و جوی معمولی
                memoryManager.SearchInMemories(searchInputField.text);
            }

            RefreshSearchButtonIcon();
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
                memoryManager.SearchInMemories(targetText);
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