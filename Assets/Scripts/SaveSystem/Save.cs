using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public class Save : MonoBehaviour
    {
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private string _saveFileName = "save.json";
        private string _saveFilePath = "";

        private SaveData _saveData;

        private void Awake()
        {
            saveButton.onClick.AddListener(SaveData);
            loadButton.onClick.AddListener(LoadData);
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

        private void SaveData()
        {
            try
            {
                var jsonContent = JsonUtility.ToJson(_saveData, true);
                File.WriteAllText(_saveFilePath, jsonContent);
            }
            catch (System.Exception e)
            {
                throw new System.Exception($"{e.Source}: {e.Message}\nSomething went wrong: Save data is failed.");
            }
        }

        private void LoadData()
        {
            if (IsSaveFileNotExist())
            {
                Debug.LogError($"[Save]: LoadData is failed. Save file is not exist. Check {Application.persistentDataPath} for containing {_saveFileName}");
                return;
            }

            try
            {
                var jsonContent = File.ReadAllText(_saveFilePath);
                JsonUtility.FromJsonOverwrite(jsonContent, _saveData);
            }
            catch (System.Exception e)
            {
                throw new System.Exception($"{e.Source}: {e.Message}\nSomething went wrong: Cant parse save data from json. Check for syntax error of jsonFile");
            }
        }

        private bool IsSaveFileNotExist() => !File.Exists(_saveFilePath);
    }
}
