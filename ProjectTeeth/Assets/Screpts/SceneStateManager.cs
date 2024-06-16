using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPositionData
    {
        public string objectName;
        public Vector3 position;
    }

    public List<ObjectPositionData> objectPositions = new List<ObjectPositionData>();

    private string saveFilePath = "saved_positions.json";

    void Start()
    {
        LoadPositions(); // Загрузка сохраненных позиций при старте сцены
    }

    public void SavePositions()
    {
        objectPositions.Clear();

        // Сохранение текущих позиций всех объектов
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Selectable"); // Меняйте тег при необходимости

        foreach (var obj in objects)
        {
            ObjectPositionData data = new ObjectPositionData();
            data.objectName = obj.name;
            data.position = obj.transform.position;
            objectPositions.Add(data);
        }

        // Сериализация данных в JSON
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Saved positions to: " + saveFilePath);
    }

    public void LoadPositions()
    {
        // Загрузка сохраненных позиций из файла JSON
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            JsonUtility.FromJsonOverwrite(json, this);

            // Применение загруженных позиций к объектам на сцене
            foreach (var data in objectPositions)
            {
                GameObject obj = GameObject.Find(data.objectName);
                if (obj != null)
                {
                    obj.transform.position = data.position;
                }
            }

            Debug.Log("Loaded positions from: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No save file found at: " + saveFilePath);
        }
    }

    public void ExitApplication()
    {
        SavePositions(); // Сохранение перед выходом
        Debug.Log("Exiting application...");
        Application.Quit();
    }
}
