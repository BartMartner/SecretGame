using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct RoomData
{
    public string name;
    public string level;
    public string fullPath;
}

public class RoomManager : MonoBehaviour 
{
    public static RoomManager instance;
    public List<RoomData> roomDatas = new List<RoomData>();
	
	private void Awake () 
    {
        instance = this;
	}

    private void OnDestroy()
    {
        instance = null;
    }
}
