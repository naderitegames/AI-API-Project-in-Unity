using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nader.ObjectPooling;
using RTLTMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoriesKeeper : MonoBehaviour
    {
        [SerializeField] MemoryManager _memoryManager;
        [SerializeField] MemoryDisplay memoryDisplayPrefab;
        [SerializeField] GameObject _noMemoriesFoundText;
        [SerializeField] RectTransform otherMemoriesPlace;
        [SerializeField] RectTransform pinnedMemoriesPlace;
        List<MemoryContainer> _memories;
        private ObjectPool<MemoryDisplay> _pinnedDisplayersPool;
        private ObjectPool<MemoryDisplay> _displayersPool;

        private void Awake()
        {
            _displayersPool =
                new ObjectPool<MemoryDisplay>(memoryDisplayPrefab, 0, otherMemoriesPlace.transform, false);
            _pinnedDisplayersPool =
                new ObjectPool<MemoryDisplay>(memoryDisplayPrefab, 0, pinnedMemoriesPlace.transform, false);
        }
        
        private void OnEnable()
        {
            RefreshDisplayers();
        }

        public void RefreshDisplayers()
        {
            _memories = _memoryManager.GetAllMemories();
            if (_memories.Count >= 1)
                RefreshDisplayers(_memories);
        }

        public void RefreshDisplayers(List<MemoryContainer> targetMemories)
        {
            if (targetMemories?.Count >= 1)
            {
                _noMemoriesFoundText.SetActive(false);
                DisableAllDisplayers();

                foreach (var t in targetMemories)
                {
                    DisplayAnotherOneMemory(t);
                }
            }
            else
            {
                DisableAllDisplayers();
                _noMemoriesFoundText.SetActive(true);
            }
        }

        void DisableAllDisplayers()
        {
            for (int i = 0; i < otherMemoriesPlace.childCount; i++)
            {
                otherMemoriesPlace.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < pinnedMemoriesPlace.childCount; i++)
            {
                pinnedMemoriesPlace.GetChild(i).gameObject.SetActive(false);
            }
        }

        void DisplayAnotherOneMemory(MemoryContainer container)
        {
            var newDisplay = container.IsPinned ? _pinnedDisplayersPool.GetObject() : _displayersPool.GetObject();
            newDisplay.transform.localScale = Vector3.one;
            newDisplay.SetUp(container, _memoryManager);
        }
    }
}