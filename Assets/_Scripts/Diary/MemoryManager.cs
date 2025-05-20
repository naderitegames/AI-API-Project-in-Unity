using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using _Scripts.AI.Gemini;
using _Scripts.Search_Box;
using _Scripts.UI;
using RTLTMPro;
using UnityEngine;

namespace _Scripts.Diary
{
    public class MemoryManager : Singleton<MemoryManager>
    {
        [SerializeField] UiManager _uiManager;
        [SerializeField] MemoriesKeeper _memoriesKeeper;
        private GeminiAiManager AiManager => GeminiAiManager.Instance;
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

        /*public void CreateMemoryFromInput(MemoryInput memoryInput)
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
                print(diary.Description);
                _data.AddMemory(diary);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }*/

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

        public void RemoveAllSelectedMemories()
        {
            UiManager.Instance.DisplayThisWarning("از حذف همه اطمینان دارید؟", () =>
            {
                foreach (var diary in MemoryDisplay.SelectedDisplayers)
                {
                    _data.Remove(diary.GetDiary());
                }

                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
                _memoriesKeeper.RefreshDisplayers();
            }, "خیر", "بله");
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
                    SaveMemoriesAndRefresh, // it will save an empty file (with one sample diary)
                    (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DisplayThisMemories(List<DiaryContainer> targets)
        {
            //if (targets is { Count: > 0 })
                _memoriesKeeper.RefreshDisplayers(targets);
            //if (targets == null || targets.Count == 0)
            //    _memoriesKeeper.RefreshDisplayers();
        }

        public void RefreshMemoriesList()
        {
            _memoriesKeeper.RefreshDisplayers();
        }
        public void SummarizeThisMemoryThenSave(DiaryContainer diary, Action<DiaryContainer> onComplete = null)
        {
            try
            {
                UiManager.Instance.DisplayThisWarning("از خلاصه کردن اطمینان دارید؟",
                    () => { TryGetSummarizeForThisDiary(diary); }, "خیر", "بله");
            }
            catch (Exception e)
            {
                UiManager.Instance.DisplayThisWarning("مشکلی در خلاصه سازی رخ داد. کنسول را چک کنید", "بسیار خب");
                print("Error : " + e);
                throw;
            }
        }

        public void SummarizeAllSelectedMemories()
        {
            try
            {
                UiManager.Instance.DisplayThisWarning("از خلاصه سازی جمعی اطمینان دارید؟", () =>
                {
                    foreach (var target in MemoryDisplay.SelectedDisplayers)
                    {
                        TryGetSummarizeForThisDiary(target.GetDiary());
                    }
                });
            }
            catch (Exception e)
            {
                UiManager.Instance.DisplayThisWarning("مشکلی در خلاصه سازی رخ داد. کنسول را چک کنید", "بسیار خب");
                print("Error : " + e);
                throw;
            }
        }

        void TryGetSummarizeForThisDiary(DiaryContainer diary, Action<DiaryContainer> OnSuccess = null)
        {
            if (!DiarySummarizer.CanSummarize(diary, _uiManager)) return;
            DiarySummarizer.TrySummarize(diary.Description, summaryLineCount, (summary) =>
            {
                // ساخت کپی برای جایگزینی
                DiaryContainer target = new DiaryContainer(diary.Description, diary.Title);
                target.UpdateId(diary.ID);
                target.UpdateSummary(summary);
                OnSuccess?.Invoke(target);
                _data.UpdateMemoryByIdOrMakeNewOne(target);
                SaveMemoriesAndRefresh();
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