using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using _Scripts.AI.Gemini;
using _Scripts.Search_Box;
using _Scripts.UI;
using RTLTMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Diary
{
    public class MemoryManager : Singleton<MemoryManager>
    {
        public List<DiaryContainer> AllMemories => _data.Diaries;
        [SerializeField] int summaryLineCount;
        [SerializeField] UiManager _uiManager;
        [SerializeField] DiariesHolder diariesHolder;
        DiariesData _data = new();
        string _diariesStorePath;

        private void Awake()
        {
            _diariesStorePath = Application.persistentDataPath + "/MemoriesData.mData";
            LoadMemories();
        }


        public void RemoveFromAllDiaries(DiaryContainer diary)
        {
            try
            {
                _data.RemoveDiary(diary);
                SaveLoadSystem.Save(_data, _diariesStorePath, () => { }, (e) => throw e);
                diariesHolder.DisplayDiaries();
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
                    _data.RemoveDiary(diary.Diary);
                }

                SaveLoadSystem.Save(_data, _diariesStorePath, () => { }, (e) => throw e);
                diariesHolder.DisplayDiaries();
            }, "خیر", "بله");
        }

        public void ToggleThisMemoryPinState(DiaryContainer diary)
        {
            GetMemoryById(diary).SetPinStateTo(!diary.IsPinned);
            SaveLoadSystem.Save(_data, _diariesStorePath, () => { }, (e) => throw e);
            diariesHolder.DisplayDiaries();
        }

        void LoadMemories()
        {
            try
            {
                SaveLoadSystem.Load<DiariesData>(_diariesStorePath,
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
            diariesHolder.DisplayDiaries(targets);
        }

        public void RefreshMemoriesList()
        {
            diariesHolder.DisplayDiaries();
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
                        TryGetSummarizeForThisDiary(target.Diary);
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
            SaveLoadSystem.Save(_data, _diariesStorePath,
                () => Debug.Log("✅ خاطره ذخیره شد."),
                (e) => Debug.LogError("❌ خطا در ذخیره‌سازی: " + e.Message));

            diariesHolder.DisplayDiaries();
        }


        DiaryContainer GetMemoryById(DiaryContainer target)
        {
            return _data.Diaries.FirstOrDefault(m => m.ID == target.ID);
        }

        public void UpdateThisEditedMemoryIfExists(DiaryContainer targetDiary)
        {
            _data.UpdateMemoryByIdOrMakeNewOne(targetDiary);
            SaveMemoriesAndRefresh();
        }
    }
}