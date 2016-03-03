using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollectionRateText : MonoBehaviour
{
    public Text collectionRate;

    public void Start()
    {
        collectionRate = GetComponent<Text>();
    }

    public void Update()
    {
        collectionRate.text = "Items Found: " + SaveGameManager.instance.saveGameData.ItemCount();
    }
}
