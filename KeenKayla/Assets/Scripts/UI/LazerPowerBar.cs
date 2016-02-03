using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LazerPowerBar : MonoBehaviour
{
    public Image lazer;
    public Image fill;
    public Image background;
    public Sprite redLazer;
    public Sprite purpleLazer;
    public Color greenColor;
    public Color redColor;
    public Color purpleColor;

    private float _lastMax;


    public void Start()
    {
        Player.instance.onRefreshPowerUps += OnRefreshPowerUp;
    }

    public void Update()
    {
        fill.fillAmount = Player.instance.lazerPower / Player.instance.maxLazerPower;

        if(_lastMax != Player.instance.maxLazerPower)
        {
            var size = background.rectTransform.sizeDelta;
            size.x = Player.instance.maxLazerPower * 10;
            background.rectTransform.sizeDelta = size;
        }

        _lastMax = Player.instance.maxLazerPower;
    }

    public void OnRefreshPowerUp()
    {
        if (SaveGameManager.instance.saveGameData.powerUpsCollected.Contains(PowerUpID.PurpleLazer))
        {
            lazer.sprite = purpleLazer;
            fill.color = purpleColor;
        }
        else if (SaveGameManager.instance.saveGameData.powerUpsCollected.Contains(PowerUpID.RedLazer))
        {
            lazer.sprite = redLazer;
            fill.color = redColor;
        }
    }
}
