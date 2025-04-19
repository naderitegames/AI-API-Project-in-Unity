using System;

namespace _Scripts
{
    [Serializable]
    public class MemoryContainer
    {
        public DateTime MemoryTime => _memoryTime;
        private DateTime _memoryTime;
        public string Title => _title;
        string _title = "";
        public string Summary => _summary;
        string _summary = "";
        public string Description => _description;
        string _description;
        public DateTime LastUpdateTime => _lastUpdateTime;
        DateTime _lastUpdateTime;

        public MemoryContainer()
        {
            _memoryTime = DateTime.Now;
            _description = "";
            _lastUpdateTime = DateTime.Now;
        }

        public MemoryContainer(string description)
        {
            _memoryTime = DateTime.Now;
            _description = description;
            _lastUpdateTime = DateTime.Now;
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
    }
}