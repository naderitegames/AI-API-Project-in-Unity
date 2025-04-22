using System;
using System.Collections.Generic;
using System.Linq;

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
        public void UpdateMemoryById(MemoryContainer updatedMemory)
        {
            for (int i = 0; i < _memories.Count; i++)
            {
                if (_memories[i].ID == updatedMemory.ID)
                {
                    _memories[i].UpdateMemoryDescription(updatedMemory.Description);
                    _memories[i].UpdateSummary(updatedMemory.Summary);
                    _memories[i].UpdateTitle(updatedMemory.Title);
                    _memories[i].SetLastUpdateTime(DateTime.Now);
                    return;
                }
            }

            UnityEngine.Debug.LogWarning($"❗ خاطره‌ای با ID {updatedMemory.ID} پیدا نشد.");
        }
    }
}