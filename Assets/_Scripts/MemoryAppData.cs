using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Scripts.Diary;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class MemoryAppData
    {
        private List<DiaryContainer> _memories;
        public List<DiaryContainer> Memories => _memories;

        public MemoryAppData()
        {
            _memories = new List<DiaryContainer>();
            //ایجاد یک خاطره به طور پیشفرض هنگام اولین ورود
            AddMemory(new DiaryContainer(
                "سلام. امروز اولین روزی هست که از این برنامه استفاده میکنی. اگر خوشت اومد یادت نره به برنامه امتیاز بدی." +
                "همینطور میتونی با دادن نظرات مفید به بهبود برنامه کمک کنی.",
                "اولین استفاده"));
        }

        public void AddMemory(DiaryContainer diary)
        {
            if (!_memories.Contains(diary))
            {
                _memories.Add(diary);
                SortAscendingByCreationDate();
            }
        }

        public void Remove(DiaryContainer diary)
        {
            if (_memories.Contains(diary))
            {
                _memories.Remove(diary);
                SortAscendingByCreationDate();
            }
        }

        void SortAscendingByCreationDate()
        {
            _memories = _memories.OrderBy(x => x.CreationDate).ToList();
        }

        public void UpdateMemoryByIdOrMakeNewOne(DiaryContainer targetDiary)
        {
            for (int i = 0; i < _memories.Count; i++)
            {
                if (_memories[i].ID == targetDiary.ID)
                {
                    Deboger.Instance.Log("در حال آپدیت مقادیر خاطره");
                    _memories[i].UpdateDiaryDescription(targetDiary.Description);
                    _memories[i].UpdateSummary(targetDiary.Summary);
                    _memories[i].UpdateTitle(string.IsNullOrEmpty(targetDiary.Title)
                        ? "خاطره شماره " + _memories.Count
                        : targetDiary.Title);
                    _memories[i].SetLastUpdateTime(DateTime.Now);
                    if (targetDiary.HasEmbedding())
                        _memories[i].SetEmbedding(targetDiary.Embedding);
                    return;
                }
            }

            if (string.IsNullOrEmpty(targetDiary.Title))
                targetDiary.UpdateTitle("خاطره شماره " + _memories.Count);
            AddMemory(targetDiary);
            Deboger.Instance.Log("diary did not find but added");
        }
    }
}