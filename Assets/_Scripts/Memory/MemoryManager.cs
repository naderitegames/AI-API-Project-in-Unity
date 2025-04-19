using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class MemoryManager : MonoBehaviour
    {
        List<MemoryContainer> _memories = new List<MemoryContainer>();

        public void AddToMemories(MemoryContainer memory)
        {
            _memories.Add(memory);
        }

        public void RemoveFromMemories(MemoryContainer memory)
        {
            _memories.Remove(memory);
        }

        public List<MemoryContainer> GetAllMemories()
        {
            return _memories;
        }
    }
}