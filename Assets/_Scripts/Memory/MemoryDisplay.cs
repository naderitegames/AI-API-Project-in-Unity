using System;
using System.Globalization;
using RTLTMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoryDisplay : MonoBehaviour
    {
        [SerializeField] private int charLimitationForDescription;
        [SerializeField] private int charLimitationForTitle;
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
                titlePlace.text = GetShortPreview(_memory.Title, charLimitationForTitle);
                var targetDescription = _memory.Summary == "" ? _memory.Description : _memory.Summary;
                descriptionPlace.text = GetShortPreview(targetDescription, charLimitationForDescription);
                creationTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
                lastModifiedTimePlace.text = _memory.LastUpdateTime.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string GetShortPreview(string text, int maxCharacters = 150)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            if (text.Length <= maxCharacters)
                return text;

            // برای اینکه کلمات نصفه نشکنن
            string shortened = text.Substring(0, maxCharacters);
            int lastSpace = shortened.LastIndexOf(' ');

            if (lastSpace > 0)
                shortened = shortened.Substring(0, lastSpace);

            return shortened + " ...";
        }
    }
}