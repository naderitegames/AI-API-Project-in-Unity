using System.Collections.Generic;
using Nader.ObjectPooling;
using UnityEngine;

namespace _Scripts.Diary
{
    public class DiariesHolder : MonoBehaviour
    {
        private MemoryManager memoryManager => MemoryManager.Instance;
        [SerializeField] MemoryDisplay memoryDisplayPrefab;
        [SerializeField] GameObject _noMemoriesFoundText;
        [SerializeField] RectTransform otherMemoriesPlace;
        [SerializeField] RectTransform pinnedMemoriesPlace;
        List<DiaryContainer> _memories;
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
            DisplayDiaries();
        }

        /// <summary>
        /// Display the Diaries in home window from all saved diaries.
        /// </summary>
        public void DisplayDiaries()
        {
            _memories = memoryManager.AllMemories;
            DisplayDiaries(_memories);
        }

        /// <summary>
        /// Display Diaries in home window but from the target list.
        /// </summary>
        public void DisplayDiaries(List<DiaryContainer> targetMemories)
        {
            DisableAllDiaryDisplayers();
            if (targetMemories?.Count >= 1)
            {
                _noMemoriesFoundText.SetActive(false);
                foreach (var t in targetMemories)
                    CreateAnotherDiary(t);
            }
            else
                _noMemoriesFoundText.SetActive(true);

            pinnedMemoriesPlace.gameObject.SetActive(_pinnedDisplayersPool.GetActiveObjects().Count > 0);
            otherMemoriesPlace.gameObject.SetActive(_otherDisplayersPool.GetActiveObjects().Count > 0);
        }

        private void DisableAllDiaryDisplayers()
        {
            for (int i = 0; i < otherMemoriesPlace.childCount; i++)
            {
                otherMemoriesPlace.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < pinnedMemoriesPlace.childCount; i++)
            {
                pinnedMemoriesPlace.GetChild(i).gameObject.SetActive(false);
            }

            // Disable diary holders (pinned or unpinned) and enable them later if they have any children
            pinnedMemoriesPlace.gameObject.SetActive(false);
            otherMemoriesPlace.gameObject.SetActive(false);
        }

        /// <summary>
        /// Create a diary displayer from pool and display it in Pinned or Unpinned list in home window.
        /// </summary>
        /// <param name="target"></param>
        private void CreateAnotherDiary(DiaryContainer target)
        {
            var newDisplay = target.IsPinned ? _pinnedDisplayersPool.GetObject() : _otherDisplayersPool.GetObject();
            newDisplay.transform.localScale = Vector3.one;
            newDisplay.SetUp(target);
        }
    }
}