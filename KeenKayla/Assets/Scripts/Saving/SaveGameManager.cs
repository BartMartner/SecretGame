using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveGameData
{
    public DateTime lastSaved;
    public Inventory inventory;
}

[Serializable]
public class Inventory
{
    public List<PowerUpType> powerUpsCollected;
    public int currentBombs;
    public int maxBombs;
    public float maxHealth;
}

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;
    public SaveGameData saveGameData = new SaveGameData();

    private string _saveGameFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        _saveGameFilePath = Application.persistentDataPath + "/SaveGame.dat";
    }

    private void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        if (File.Exists(_saveGameFilePath))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(_saveGameFilePath, FileMode.Open);
            saveGameData = (SaveGameData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        saveGameData = new SaveGameData();
        SaveGame();
    }

    public void SaveGame()
    {
        var bf = new BinaryFormatter();
        var file = File.Create(_saveGameFilePath);
        saveGameData.lastSaved = DateTime.Now;
        bf.Serialize(file, saveGameData);
        file.Close();
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }
}
