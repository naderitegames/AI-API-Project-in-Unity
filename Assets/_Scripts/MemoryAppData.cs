using System;
using System.Collections.Generic;

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
                _memories.Add(memory);
        }

        public void Remove(MemoryContainer memory)
        {
            if (_memories.Contains(memory))
                _memories.Remove(memory);
        }
    }
}