using System;

namespace _Scripts.Diary
{
    [Serializable]
    public class DiaryContainer
    {
        public string ID => _id;
        private string _id;
        public DateTime MemoryTime => _memoryTime;
        private DateTime _memoryTime;
        public string Title => _title;
        private string _title = "";
        public string Summary => _summary;
        private string _summary = "";
        public string Description => _description;
        private string _description;
        public DateTime TargetDate => _targetDate;
        private DateTime _targetDate;
        public DateTime LastUpdateTime => _lastUpdateTime;
        private DateTime _lastUpdateTime;
        public int TotalCharactersInMemory => _totalCharactersInMemory;
        private int _totalCharactersInMemory;
        public bool IsPinned => _isPinned;
        private bool _isPinned = false;

        public float[] Embedding => _embedding;
        private float[] _embedding;

        public DiaryContainer()
        {
            InitializeNewMemory("", "");
        }

        public DiaryContainer(string description, string title)
        {
            InitializeNewMemory(title, description);
        }

        void InitializeNewMemory(string title, string description)
        {
            _id = Guid.NewGuid().ToString();
            _title = title;
            _description = description;
            _memoryTime = DateTime.Now;
            _lastUpdateTime = _targetDate = DateTime.Now;
            UpdateMemoryProperties();
        }

        void UpdateMemoryProperties()
        {
            _totalCharactersInMemory = _description.Length + _title.Length;
        }

        public void UpdateId(string newId)
        {
            _id = newId;
        }

        public void UpdateDiaryDescription(string description)
        {
            if (!string.Equals(_description, description, StringComparison.Ordinal))
            {
                _description = description;
                _lastUpdateTime = DateTime.Now;
                ClearEmbedding();
                UpdateMemoryProperties();
            }
            
        }

        public void UpdateTitle(string newTitle)
        {
            if (!string.Equals(_title, newTitle, StringComparison.Ordinal))
            {
                _title = newTitle;
                _lastUpdateTime = DateTime.Now;
                ClearEmbedding();
                UpdateMemoryProperties();
            }
        }

        public void UpdateSummary(string summary)
        {
            _summary = summary;
        }

        public void SetLastUpdateTime(DateTime time)
        {
            _lastUpdateTime = time;
        }

        public void SetPinStateTo(bool b)
        {
            _isPinned = b;
        }

        public void SetEmbedding(float[] newEmbedding)
        {
            _embedding = newEmbedding;
        }

        public void ClearEmbedding()
        {
            _embedding = null;
        }

        public bool HasEmbedding()
        {
            return _embedding is { Length: > 767 };
        }
    }
}
