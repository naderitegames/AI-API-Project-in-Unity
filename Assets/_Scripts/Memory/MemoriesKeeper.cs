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
        private MemoryManager memoryManager => MemoryManager.Instance;
        [SerializeField] MemoryDisplay memoryDisplayPrefab;
        [SerializeField] GameObject _noMemoriesFoundText;
        [SerializeField] RectTransform otherMemoriesPlace;
        [SerializeField] RectTransform pinnedMemoriesPlace;
        List<MemoryContainer> _memories;
        private ObjectPool<MemoryDisplay> _pinnedDisplayersPool;
        private ObjectPool<MemoryDisplay> _otherDisplayersPool;

        private void Awake()
        {
            _otherDisplayersPool =
                new ObjectPool<MemoryDisplay>(memoryDisplayPrefab, 0, otherMemoriesPlace.transform, false);
            _pinnedDisplayersPool =
                new ObjectPool<MemoryDisplay>(memoryDisplayPrefab, 0, pinnedMemoriesPlace.transform, false);
        }

        private void Start()
        {
            RefreshDisplayers();
        }

        public void RefreshDisplayers()
        {
            _memories = memoryManager.GetAllMemories();
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

            pinnedMemoriesPlace.gameObject.SetActive(_pinnedDisplayersPool.GetActiveObjects().Count > 0);
            otherMemoriesPlace.gameObject.SetActive(_otherDisplayersPool.GetActiveObjects().Count > 0);
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

            pinnedMemoriesPlace.gameObject.SetActive(false);
            otherMemoriesPlace.gameObject.SetActive(false);
        }

        void DisplayAnotherOneMemory(MemoryContainer target)
        {
            var newDisplay = target.IsPinned ? _pinnedDisplayersPool.GetObject() : _otherDisplayersPool.GetObject();
            newDisplay.transform.localScale = Vector3.one;
            newDisplay.SetUp(target);
        }
    }
}