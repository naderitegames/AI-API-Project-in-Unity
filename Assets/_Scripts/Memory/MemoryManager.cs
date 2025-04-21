using System;
using System.Collections.Generic;
using System.Data;
using RTLTMPro;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class MemoryManager : MonoBehaviour
    {
        [SerializeField] UiManager _uiManager;
        [SerializeField] MemoriesKeeper _memoriesKeeper;
        MemoryAppData _data = new MemoryAppData();
        private string _memoryDataPath;

        private void Awake()
        {
            _memoryDataPath = Application.persistentDataPath + "/MemoriesData.mData";
            LoadMemories();
        }

        public List<MemoryContainer> GetAllMemories()
        {
            return _data.Memories;
        }

        public void CreateMemoryFromInput(MemoryInput memoryInput)
        {
            try
            {
                var description = memoryInput.Description;
                var title = memoryInput.Ttle;
                if (title == "")
                {
                    title = "خاطره شماره " + _data.Memories.Count;
                }

                if (description == "")
                {
                    throw new NoNullAllowedException(" Memory description can not be empty");
                }

                memoryInput.RefreshInputs();
                AddToMemoriesAndSave(new MemoryContainer(description, title));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        void AddToMemoriesAndSave(MemoryContainer memory)
        {
            try
            {
                _data.AddMemory(memory);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void RemoveFromMemories(MemoryContainer memory)
        {
            try
            {
                _data.Remove(memory);
                SaveLoadSystem.Save(_data, _memoryDataPath, () => { }, (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void LoadMemories()
        {
            try
            {
                SaveLoadSystem.Load<MemoryAppData>(_memoryDataPath, (d) => { _data = d; }, () => { }, () => { },
                    (e) => throw e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SearchInMemories(TMP_InputField inputField)
        {
            var results = MemorySearch.SearchMemoriesByKeyword(inputField.text, _data.Memories);
            _memoriesKeeper.RefreshDisplayers(results);
        }
    }
}