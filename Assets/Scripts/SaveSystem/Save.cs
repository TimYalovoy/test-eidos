using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public sealed class Save : MonoBehaviour, ISaver
    {
        public SaveData Data { get; set; }

        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private string _saveFileName = "save.json";
        private string _saveFilePath = "";

        private List<ISaveable> saveables = new List<ISaveable>();

        public event Action<SaveData> SaveIsLoaded;

        private void Awake()
        {
            saveButton.onClick.AddListener(SaveData);
            loadButton.onClick.AddListener(LoadData);

            var objs = FindObjectsOfType<MonoBehaviour>();
            foreach (var obj in objs)
            {
                if (obj is ISaveable)
                {
                    saveables.Add(obj as ISaveable);
                }
            }

            Data = new SaveData();
        }

        private void Start()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, _saveFileName);
        }

        private void OnDestroy()
        {
            saveButton.onClick.RemoveListener(SaveData);
            loadButton.onClick.RemoveListener(LoadData);
        }

        public void SaveData()
        {
            saveables.ForEach(saveable => saveable.SetData(this));
            try
            {
                var jsonContent = JsonUtility.ToJson(Data, true);
                File.WriteAllText(_saveFilePath, jsonContent);

            }
            catch (System.Exception e)
            {
                throw new System.Exception($"{e.Source}: {e.Message}\nSomething went wrong: Save data is failed.");
            }
        }

        public void LoadData()
        {
            if (IsSaveFileNotExist())
            {
                Debug.LogError($"[Save]: LoadData is failed. Save file is not exist. Check {Application.persistentDataPath} for containing {_saveFileName}");
                return;
            }

            try
            {
                var jsonContent = File.ReadAllText(_saveFilePath);
                JsonUtility.FromJsonOverwrite(jsonContent, Data);

                SaveIsLoaded.Invoke(Data);
            }
            catch (System.Exception e)
            {
                throw new System.Exception($"{e.Source}: {e.Message}\nSomething went wrong: Cant parse save data from json. Check for syntax error of jsonFile");
            }
        }

        public void RemoveSaveFile()
        {
            if (IsSaveFileNotExist())
            {
                Debug.Log($"Save file already removed or unless is not created.");
                return;
            }

            File.Delete(_saveFilePath);
            Debug.Log($"Save file <color=green>successfull</color> removed");
        }

        private bool IsSaveFileNotExist() => !File.Exists(_saveFilePath);
    }
}
