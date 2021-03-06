﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveGameData
{
    public Vector3Data savePosition;
    public string lastRoom;
    public List<int> bombUpgradesCollected = new List<int>();
    public List<int> healthUpgradesCollected = new List<int>();
    public List<int> lazerPowerUpgradesCollected = new List<int>();
    public List<PowerUpID> powerUpsCollected = new List<PowerUpID>();
    public List<MapRoom> mapRooms = new List<MapRoom>();
    public TimeSpan playTime;

    public int CompletionRate()
    {
        //7 Power Ups;
        //9 Heart Tanks;
        //7 Lazer Upgrades;
        //4 Bomb Upgrade;
        var totalItems = 7f + 9f + 7f + 4f;
        var collected = bombUpgradesCollected.Count + healthUpgradesCollected.Count + lazerPowerUpgradesCollected.Count + powerUpsCollected.Count;
        return (int)((collected/totalItems) * 100);
    }

    public string ItemCount()
    {
        //7 Power Ups;
        //9 Heart Tanks;
        //7 Lazer Upgrades;
        //4 Bomb Upgrade;
        var totalItems = 7f + 9f + 7f + 4f;
        var collected = bombUpgradesCollected.Count + healthUpgradesCollected.Count + lazerPowerUpgradesCollected.Count + powerUpsCollected.Count;
        return collected +"/"+totalItems;
    }
}

[Serializable]
public struct MapRoom
{
    public Vector3Data position;
    public Vector3Data size;

    public override bool Equals(object obj)
    {
        return obj is MapRoom && this == (MapRoom)obj;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode() ^ size.GetHashCode();
    }

    public static bool operator ==(MapRoom a, MapRoom b)
    {
        return a.position == b.position && a.size == b.size;
    }
    public static bool operator !=(MapRoom a, MapRoom b)
    {
        return !(a == b);
    }
}

[Serializable]
public struct Vector3Data
{
    public Vector3Data(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float x;
    public float y;
    public float z;

    public override bool Equals(object obj)
    {
        return obj is Vector3Data && this == (Vector3Data)obj;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
    }

    public static bool operator ==(Vector3Data a, Vector3Data b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(Vector3Data a, Vector3Data b)
    {
        return !(a == b);
    }
}

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;
    public SaveGameData saveGameData = new SaveGameData();
    public DateTime sessionStart;

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

        LoadGame();
    }

    public void Start()
    {
        if (Player.instance && !Player.instance.ignoreSavePosition)
        {
            SceneManager.LoadScene(saveGameData.lastRoom, LoadSceneMode.Additive);
        }
    }

    public void LoadGame()
    {
        sessionStart = DateTime.UtcNow;
        if (File.Exists(_saveGameFilePath))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(_saveGameFilePath, FileMode.Open);
            saveGameData = (SaveGameData)bf.Deserialize(file);
            file.Close();

            if (saveGameData.lazerPowerUpgradesCollected == null)
            {
                saveGameData.lazerPowerUpgradesCollected = new List<int>();
            }

            if (saveGameData.mapRooms == null)
            {
                saveGameData.mapRooms = new List<MapRoom>();
            }
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        saveGameData = new SaveGameData();
        saveGameData.savePosition = new Vector3Data(0, 3, 0);
        saveGameData.lastRoom = "Entryway";
        SaveGame();
        Time.timeScale = 1;
    }

    public void SaveGame()
    {
        saveGameData.playTime += DateTime.UtcNow - sessionStart;
        sessionStart = DateTime.UtcNow;
        var bf = new BinaryFormatter();
        var file = File.Create(_saveGameFilePath);
        bf.Serialize(file, saveGameData);
        file.Close();
    }
}

