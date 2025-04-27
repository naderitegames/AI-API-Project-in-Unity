using System;
using System.Collections.Generic;
using System.Linq;
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
            AddMemory(new DiaryContainer("امروز اولین روزی هست که از این برنامه استفاده میکنم.", "اولین استفاده"));
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
            _memories = _memories.OrderBy(x => x.TargetDate).ToList();
        }

        public void UpdateMemoryByIdOrMakeNewOne(DiaryContainer targetDiary)
        {
            for (int i = 0; i < _memories.Count; i++)
            {
                if (_memories[i].ID == targetDiary.ID)
                {
                    _memories[i].UpdateMemoryDescription(targetDiary.Description);
                    _memories[i].UpdateSummary(targetDiary.Summary);
                    _memories[i].UpdateTitle(targetDiary.Title == ""
                        ? "خاطره شماره " + _memories.Count
                        : targetDiary.Title);
                    _memories[i].SetLastUpdateTime(DateTime.Now);
                    return;
                }
            }

            AddMemory(targetDiary);
            Debug.Log("diary did not find but added");
        }
    }
}