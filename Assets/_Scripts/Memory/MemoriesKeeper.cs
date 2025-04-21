using System;
using System.Collections.Generic;
using Nader.ObjectPooling;
using UnityEngine;

namespace _Scripts
{
    public class MemoriesKeeper : MonoBehaviour
    {
        [SerializeField] MemoryManager _memoryManager;
        [SerializeField] MemoryDisplay memoryDisplayPrefab;
        List<MemoryContainer> _memories;
        private ObjectPool<MemoryDisplay> _displayersPool;

        private void Awake()
        {
            _displayersPool = new ObjectPool<MemoryDisplay>(memoryDisplayPrefab, 5, transform, false);
        }

        private void OnEnable()
        {
            RefreshDisplayers();
        }

        public void RefreshDisplayers()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            _memories = _memoryManager.GetAllMemories();
            for (int i = 0; i < _memories.Count; i++)
            {
                var newDisplayer = _displayersPool.GetObject();
                newDisplayer.transform.localScale = Vector3.one;
                newDisplayer.UpdateMemory(_memories[i]);
            }
        }
    }
}