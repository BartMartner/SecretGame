using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayTimeText : MonoBehaviour
{
    private Text _text;

    public void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void Update()
    {
        var time = SaveGameManager.instance.saveGameData.playTime;
        _text.text = time.Hours.ToString("D2") + ":" + time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2");
    }
}
