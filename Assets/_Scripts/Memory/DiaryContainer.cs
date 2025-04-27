using System;
using UnityEngine.Networking.Types;

namespace _Scripts
{
    [Serializable]
    public class DiaryContainer
    {
        public string ID => _id;
        private string _id;
        public DateTime MemoryTime => _memoryTime;
        private DateTime _memoryTime;
        public string Title => _title;
        string _title = "";
        public string Summary => _summary;
        string _summary = "";
        public string Description => _description;
        string _description;
        public DateTime TargetDate => _targetDate;
        DateTime _targetDate;
        public DateTime LastUpdateTime => _lastUpdateTime;
        DateTime _lastUpdateTime;
        public int TotalCharactersInMemory => _totalCharactersInMemory;
        int _totalCharactersInMemory;
        public bool IsPinned => _isPinned;
        bool _isPinned = false;

        public DiaryContainer()
        {
            InitializeNewMemory("", "");
        }

        public void UpdateId(string newId)
        {
            _id = newId;
        }

        public DiaryContainer(string description, string title)
        {
            InitializeNewMemory(title, description);
        }

        public void UpdateMemoryDescription(string description)
        {
            _lastUpdateTime = DateTime.Now;
            _description = description;
        }

        public void UpdateSummary(string summary)
        {
            _summary = summary;
        }

        void InitializeNewMemory(string title, string description)
        {
            _id = Guid.NewGuid().ToString();
            _title = title;
            _memoryTime = DateTime.Now;
            _description = description;
            _lastUpdateTime = _targetDate = DateTime.Now;
            UpdateMemoryProperties();
        }

        void UpdateMemoryProperties()
        {
            _totalCharactersInMemory = _description.Length + _title.Length;
        }

        public void UpdateTitle(string newTitle)
        {
            _title = newTitle;
            _lastUpdateTime = DateTime.Now;
        }

        public void SetLastUpdateTime(DateTime time)
        {
            _lastUpdateTime = time;
        }

        public void SetPinStateTo(bool b)
        {
            _isPinned = b;
        }
    }
}