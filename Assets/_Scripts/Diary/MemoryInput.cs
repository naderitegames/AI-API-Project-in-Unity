using TMPro;
using UnityEngine;

namespace _Scripts.Diary
{
    public class MemoryInput : MonoBehaviour
    {
        [SerializeField] TMP_InputField title, description;
        public string Description => description.text;
        public string Ttle => title.text;

        public void RefreshInputs()
        {
            title.text = description.text = "";
        }

        public void ClearDescription()
        {
            description.text = "";
        }

        public void ForceInputValuesTo()
        {
        
        }
    }
}