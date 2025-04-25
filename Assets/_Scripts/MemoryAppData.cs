using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class MemoryAppData
    {
        private List<MemoryContainer> _memories;
        public List<MemoryContainer> Memories => _memories;

        public MemoryAppData()
        {
            _memories = new List<MemoryContainer>();
            AddMemory(new MemoryContainer("امروز اولین روزی هست که از این برنامه استفاده میکنم.", "اولین استفاده"));
        }

        public void AddMemory(MemoryContainer memory)
        {
            if (!_memories.Contains(memory))
            {
                _memories.Add(memory);
                SortAscendingByCreationDate();
            }
        }

        public void Remove(MemoryContainer memory)
        {
            if (_memories.Contains(memory))
            {
                _memories.Remove(memory);
                SortAscendingByCreationDate();
            }
        }

        void SortAscendingByCreationDate()
        {
            _memories = _memories.OrderBy(x => x.TargetDate).ToList();
        }

        public void UpdateMemoryByIdOrMakeNewOne(MemoryContainer targetMemory)
        {
            for (int i = 0; i < _memories.Count; i++)
            {
                if (_memories[i].ID == targetMemory.ID)
                {
                    _memories[i].UpdateMemoryDescription(targetMemory.Description);
                    _memories[i].UpdateSummary(targetMemory.Summary);
                    _memories[i].UpdateTitle(targetMemory.Title);
                    _memories[i].SetLastUpdateTime(DateTime.Now);
                    return;
                }
            }

            AddMemory(targetMemory);
            Debug.Log("diary did not find but added");
            //UnityEngine.Debug.LogWarning($"❗ خاطره‌ای با ID {targetMemory.ID} پیدا نشد.");
        }
    }
}