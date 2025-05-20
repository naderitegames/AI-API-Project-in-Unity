using System;

namespace _Scripts.Diary
{
    [Serializable]
    public class DiaryContainer
    {
        public string ID => _id;
        private string _id;
        public string Title => _title;
        private string _title = "";
        public string Summary => _summary;
        private string _summary = "";
        public string Description => _description;
        private string _description;
        public DateTime CreationDate => _creationDate;
        private DateTime _creationDate;
        public DateTime LastUpdateTime => _lastUpdateTime;
        private DateTime _lastUpdateTime;
        private int _totalCharactersInDiary;
        public bool IsPinned => _isPinned;
        private bool _isPinned = false;

        public float[] Embedding => _embedding;
        private float[] _embedding;

        public DiaryContainer()
        {
            InitializeNewDiary("", "");
        }

        public DiaryContainer(string description, string title)
        {
            InitializeNewDiary(title, description);
        }

        void InitializeNewDiary(string title, string description)
        {
            _id = Guid.NewGuid().ToString();
            _creationDate = DateTime.Now;
            UpdateDiary(title, description);
        }


        public void UpdateDescription(string description)
        {
            if (!string.Equals(_description, description, StringComparison.Ordinal))
            {
                UpdateDiary(_title, description);
            }
        }

        public void UpdateTitle(string newTitle)
        {
            if (!string.Equals(_title, newTitle, StringComparison.Ordinal))
            {
                UpdateDiary(newTitle, _description);
            }
        }

        public void UpdateId(string newId) => _id = newId;

        void UpdateDiary(string title, string description)
        {
            ClearEmbedding();
            _title = title;
            _description = description;
            _lastUpdateTime = DateTime.Now;
            _totalCharactersInDiary = _description.Length + _title.Length;
        }

        public void UpdateSummary(string summary) => _summary = summary;

        public void SetPinStateTo(bool b) => _isPinned = b;

        public void SetEmbedding(float[] newEmbedding) => _embedding = newEmbedding;

        public void ClearEmbedding() => _embedding = null;

        public bool HasEmbedding() => _embedding is { Length: > 767 };
    }
}