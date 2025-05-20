using System;
using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Diary
{
    [Serializable]
    public class DiariesData
    {
        private List<DiaryContainer> _diaries;
        public List<DiaryContainer> Diaries => _diaries;

        // Default constructor
        public DiariesData()
        {
            _diaries = new List<DiaryContainer>();
            // Create this one as welcome message ... like default pre-made diary.
            AddDiary(new DiaryContainer(
                "سلام." +
                "\n\n" +
                " امروز اولین روزی هست که از این برنامه استفاده میکنی. اگر خوشت اومد یادت نره به برنامه امتیاز بدی." +
                "\n\n" +
                "همینطور میتونی با دادن نظرات مفید به بهبود برنامه کمک کنی.",
                "اولین استفاده"));
        }

        /// <summary>
        /// Add a Diary to all diaries list.
        /// </summary>
        /// <param name="diary"></param>
        public void AddDiary(DiaryContainer diary)
        {
            if (!_diaries.Contains(diary))
            {
                _diaries.Add(diary);
                SortAscendingByCreationDate();
            }
        }

        /// <summary>
        /// Remove a diary from diaries if exists before.
        /// </summary>
        /// <param name="diary"></param>
        public void RemoveDiary(DiaryContainer diary)
        {
            if (_diaries.Contains(diary))
            {
                _diaries.Remove(diary);
                SortAscendingByCreationDate();
            }
        }

        void SortAscendingByCreationDate()
        {
            _diaries = _diaries.OrderBy(x => x.CreationDate).ToList();
        }

        /// <summary>
        /// Update an existing diary values or make a new one if it does not exist before.
        /// </summary>
        /// <param name="targetDiary"></param>
        // it is better to separate jobs of this method ... 
        public void UpdateMemoryByIdOrMakeNewOne(DiaryContainer targetDiary)
        {
            foreach (var t in _diaries.Where(t => t.ID == targetDiary.ID))
            {
                Deboger.Instance.Log("در حال آپدیت مقادیر خاطره");
                t.UpdateDescription(targetDiary.Description);
                t.UpdateSummary(targetDiary.Summary);
                t.UpdateTitle(string.IsNullOrEmpty(targetDiary.Title)
                    ? "خاطره شماره " + _diaries.Count
                    : targetDiary.Title);
                if (targetDiary.HasEmbedding())
                    t.SetEmbedding(targetDiary.Embedding);
                return;
            }

            if (string.IsNullOrEmpty(targetDiary.Title))
                targetDiary.UpdateTitle("خاطره شماره " + _diaries.Count);
            AddDiary(targetDiary);
            Deboger.Instance.Log("diary did not find but added");
        }
    }
}