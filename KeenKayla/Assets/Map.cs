using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    private int roomCount;
    public void LateUpdate()
    {
        if (roomCount != SaveGameManager.instance.saveGameData.mapRooms.Count)
        {
            roomCount = SaveGameManager.instance.saveGameData.mapRooms.Count;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var bounds in SaveGameManager.instance.saveGameData.mapRooms)
            {
                var room = new GameObject().AddComponent<Image>();
                room.color = Color.black;
                room.transform.parent = transform;
                room.transform.localScale = Vector3.one;                
                room.rectTransform.sizeDelta = bounds.size.ToVector3() * 0.48f;
                room.rectTransform.anchoredPosition = bounds.position.ToVector3() * 0.5f;
            }
        }
    }
}
