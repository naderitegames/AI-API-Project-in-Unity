using System;
using System.Collections.Generic;
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
        List<MemoryContainer> _memories;
        private ObjectPool<MemoryDisplay> _displayersPool;
        [SerializeField] GameObject _noMemoriesFoundText;
        [SerializeField] RectTransform contentPlace;

        private void Awake()
        {
            _displayersPool = new ObjectPool<MemoryDisplay>(memoryDisplayPrefab, 20, contentPlace, false);
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
            for (int i = 0; i < contentPlace.childCount; i++)
            {
                contentPlace.GetChild(i).gameObject.SetActive(false);
            }
        }

        void DisplayAnotherOneMemory(MemoryContainer container)
        {
            var newDisplay = _displayersPool.GetObject();
            newDisplay.transform.localScale = Vector3.one;
            newDisplay.SetUp(container, _memoryManager);
        }
    }
}