using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoryManager : MonoBehaviour
    {
        List<MemoryContainer> _memories = new List<MemoryContainer>();

        void AddToMemories(MemoryContainer memory)
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

        public void CreateMemoryFromInput(RTLTextMeshPro newText)
        {
            AddToMemories(new MemoryContainer(newText.text));
        }
    }
}