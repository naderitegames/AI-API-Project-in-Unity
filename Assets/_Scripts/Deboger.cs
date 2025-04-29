using UnityEngine;

namespace _Scripts
{
    public class Deboger : Singleton<Deboger>
    {
        [SerializeField] private bool canDeBog = true;

        public void LogWarning(string message)
        {
            if (!canDeBog)
                return;
            Debug.LogWarning(message);
        }

        public void Log(string message)
        {
            if (!canDeBog)
                return;
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            if (!canDeBog)
                return;
            Debug.LogError(message);
        }

        public void ForceLog(string message)
        {
            Debug.Log(message);
        }

        public void ForceLogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void ForceLogError(string message)
        {
            Debug.LogError(message);
        }
    }
}