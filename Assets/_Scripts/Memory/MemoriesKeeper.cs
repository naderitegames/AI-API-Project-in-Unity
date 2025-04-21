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
        bool isResultFound = false;

        public bool IsResultFound
        {
            private set { isResultFound = value; }
            get
            {
                _noMemoriesFoundText.SetActive(!IsResultFound);
                return isResultFound;
            }
        }

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
            _memories = _memoryManager.GetAllMemories();
            RefreshDisplayers(_memories);
        }

        public void RefreshDisplayers(List<MemoryContainer> targetMemories)
        {
            if (targetMemories?.Count >= 0)
            {
                IsResultFound = true;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < targetMemories.Count; i++)
                {
                    var newDisplayer = _displayersPool.GetObject();
                    newDisplayer.transform.localScale = Vector3.one;
                    newDisplayer.UpdateMemory(targetMemories[i]);
                }
            }
            else
            {
                IsResultFound = false;
            }
        }
    }
}