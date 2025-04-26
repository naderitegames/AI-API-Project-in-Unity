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
        [SerializeField] private GeminiAiManager _aiManager;
        [SerializeField] private int summaryLineCount;
        MemoryAppData _data = new MemoryAppData();
        private string _memoryDataPath;

        private void Awake()
        {
            _memoryDataPath = Application.persistentDataPath + "/MemoriesData.mData";
            LoadMemories();
        }

        public List<MemoryContainer> GetAllMemories()
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
                AddToMemoriesAndSave(new MemoryContainer(description, title));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        void AddToMemoriesAndSave(MemoryContainer memory)
        {
            try
            {
                _data.AddMemory(memory);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void RemoveFromMemories(MemoryContainer memory)
        {
            try
            {
                _data.Remove(memory);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
                _memoriesKeeper.RefreshDisplayers();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ToggleThisMemoryPinState(MemoryContainer memory)
        {
            GetMemoryById(memory).SetPinStateTo(!memory.IsPinned);
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

        public void SummarizeThisMemoryThenSave(MemoryContainer memory ,Action<MemoryContainer> onComplete = null)
        {
            if (!MemorySummarizer.CanSummarize(memory, _uiManager)) return;

            MemorySummarizer.TrySummarizeWithConfirmation(memory, _aiManager, _uiManager, summaryLineCount, (summary) =>
            {
                // ساخت کپی برای جایگزینی
                MemoryContainer updated = new MemoryContainer(memory.Description, memory.Title);
                updated.UpdateId(memory.ID);
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


        MemoryContainer GetMemoryById(MemoryContainer target)
        {
            return _data.Memories.FirstOrDefault(m => m.ID == target.ID);
        }

        public void UpdateThisEditedMemoryIfExists(MemoryContainer targetMemory)
        {
            _data.UpdateMemoryByIdOrMakeNewOne(targetMemory);
            SaveMemoriesAndRefresh();
        }
    }
}