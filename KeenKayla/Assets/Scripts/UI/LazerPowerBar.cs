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
    private bool _flashing;

    public void Start()
    {
        Player.instance.onRefreshPowerUps += OnRefreshPowerUp;
        OnRefreshPowerUp();
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

        if(Player.instance.lazerPower >= 4 && !_flashing)
        {
            _flashing = true;
            StartCoroutine(Flash());
        }
    }

    public IEnumerator Flash()
    {
        var originalColor = fill.color;
        var flashColor = originalColor;
        flashColor.g += 0.5f;
        flashColor.r += 0.5f;
        flashColor.b += 0.25f;

        while (Player.instance.lazerPower >= 4)
        {
            fill.color = flashColor;
            yield return new WaitForSeconds(0.1f);

            fill.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        OnRefreshPowerUp();
        _flashing = false;
    }

    public void OnRefreshPowerUp()
    {
        _flashing = false;
        StopAllCoroutines();
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
