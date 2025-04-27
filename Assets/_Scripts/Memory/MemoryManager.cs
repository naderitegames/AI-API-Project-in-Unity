using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RTLTMPro;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoryManager : Singleton<MemoryManager>
    {
        [SerializeField] UiManager _uiManager;
        [SerializeField] MemoriesKeeper _memoriesKeeper;
        private GeminiAiManager AiManager=>GeminiAiManager.Instance;
        [SerializeField] private int summaryLineCount;
        MemoryAppData _data = new MemoryAppData();
        private string _memoryDataPath;

        private void Awake()
        {
            _memoryDataPath = Application.persistentDataPath + "/MemoriesData.mData";
            LoadMemories();
        }

        public List<DiaryContainer> GetAllMemories()
        {
            return _data.Memories;
        }

        public void CreateMemoryFromInput(MemoryInput memoryInput)
        {
            try
            {
                var description = memoryInput.Description;
                var title = memoryInput.Ttle;
                if (title == "")
                {
                    title = "خاطره شماره " + _data.Memories.Count;
                }

                if (description == "")
                {
                    throw new NoNullAllowedException(" Memory description can not be empty");
                }

                memoryInput.RefreshInputs();
                AddToMemoriesAndSave(new DiaryContainer(description, title));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        void AddToMemoriesAndSave(DiaryContainer diary)
        {
            try
            {
                _data.AddMemory(diary);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void RemoveFromMemories(DiaryContainer diary)
        {
            try
            {
                _data.Remove(diary);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
                _memoriesKeeper.RefreshDisplayers();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ToggleThisMemoryPinState(DiaryContainer diary)
        {
            GetMemoryById(diary).SetPinStateTo(!diary.IsPinned);
            SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
            _memoriesKeeper.RefreshDisplayers();
        }

        void LoadMemories()
        {
            try
            {
                SaveLoadSystem.Load<MemoryAppData>(_memoryDataPath,
                    (d) => { _data = d; },
                    () => { },
                    SaveMemoriesAndRefresh,// it will save an empty file (with one sample diary)
                    (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SearchInMemories(TMP_InputField inputField)
        {
            var results = MemorySearch.SearchMemoriesByKeyword(inputField.text, _data.Memories);
            if (inputField.text != "")
                _memoriesKeeper.RefreshDisplayers(results);
            if (inputField.text == "")
                _memoriesKeeper.RefreshDisplayers();
        }

        public void SummarizeThisMemoryThenSave(DiaryContainer diary ,Action<DiaryContainer> onComplete = null)
        {
            if (!MemorySummarizer.CanSummarize(diary, _uiManager)) return;

            MemorySummarizer.TrySummarizeWithConfirmation(diary, AiManager, _uiManager, summaryLineCount, (summary) =>
            {
                // ساخت کپی برای جایگزینی
                DiaryContainer updated = new DiaryContainer(diary.Description, diary.Title);
                updated.UpdateId(diary.ID);
                updated.UpdateSummary(summary);
                _data.UpdateMemoryByIdOrMakeNewOne(updated);
                SaveMemoriesAndRefresh();
                onComplete?.Invoke(updated);
            });
        }

        private void SaveMemoriesAndRefresh()
        {
            SaveLoadSystem.Save(_data, _memoryDataPath,
                () => Debug.Log("✅ خاطره ذخیره شد."),
                (e) => Debug.LogError("❌ خطا در ذخیره‌سازی: " + e.Message));

            _memoriesKeeper.RefreshDisplayers();
        }


        DiaryContainer GetMemoryById(DiaryContainer target)
        {
            return _data.Memories.FirstOrDefault(m => m.ID == target.ID);
        }

        public void UpdateThisEditedMemoryIfExists(DiaryContainer targetDiary)
        {
            _data.UpdateMemoryByIdOrMakeNewOne(targetDiary);
            SaveMemoriesAndRefresh();
        }
    }
}