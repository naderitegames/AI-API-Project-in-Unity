using System;
using System.Globalization;
using RTLTMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoryDisplay : MonoBehaviour
    {
        [SerializeField] RTLTextMeshPro descriptionPlace;
        [SerializeField] RTLTextMeshPro titlePlace;
        [SerializeField] RTLTextMeshPro creationTimePlace;
        [SerializeField] RTLTextMeshPro lastModifiedTimePlace;
        private MemoryContainer _memory;

        private void OnEnable()
        {
            RefreshMemory();
        }

        public void UpdateMemory(MemoryContainer memory)
        {
            _memory = memory;
            RefreshMemory();
        }

        void RefreshMemory()
        {
            if (_memory != null)
            {
                titlePlace.text = _memory.Title;
                descriptionPlace.text = _memory.Description;
                creationTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
                lastModifiedTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}