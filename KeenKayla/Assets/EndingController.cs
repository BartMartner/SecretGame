using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    public Text clearTime;
    public Text collectionRate;

    // Use this for initialization
    void Start ()
    {
        var time = SaveGameManager.instance.saveGameData.playTime;
        clearTime.text = "Clear Time: " + time.Hours.ToString("D2") + ":" + time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2");
        collectionRate.text = "Items Found: " + SaveGameManager.instance.saveGameData.CompletionRate() + "%";
    }
}
