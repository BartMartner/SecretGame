using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Sprite heart;
    public Sprite halfHeart;
    public Sprite empty;
    public Image[] hearts;
    public Image[] shields;
    private float _lastValue;
    private float _lastMaxValue;
    private bool _lastCold;

    private void Awake()
    {
        int i = 0;
        foreach (var heart in hearts)
        {
            Material mat = new Material(Shader.Find("Sprites/DefaultFlash"));
            mat.name = "heart" + i;
            heart.material = mat;
            i++;
        }
    }

    private void Start()
    {
        SetHearts();
        SetShield();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_lastValue != Player.instance.health || _lastMaxValue != Player.instance.maxHealth)
        {
            _lastValue = Player.instance.health;
            _lastMaxValue = Player.instance.maxHealth;
            SetHearts();
        }

        if (Player.instance.inColdZone && !Player.instance.hasColdSuit && Player.instance.state == DamagableState.Alive)
        {
            int lastHeartIndex = (int)Mathf.Clamp(Mathf.Ceil(Player.instance.health - 1), 0, hearts.Length - 1);
            var lastHeart = hearts[lastHeartIndex];
            lastHeart.material.SetColor("_FlashColor", Color.cyan);
            lastHeart.material.SetFloat("_FlashAmount", Player.instance.coldRatio * 0.5f);
        }

        if (_lastCold && !Player.instance.inColdZone)
        {
            SetHearts();
        }

        if(Player.instance.hasPowerSuit)
        {
            SetShield();
        }

        _lastCold = Player.instance.inColdZone;
    }

    private void SetHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].material.SetFloat("_FlashAmount", 0);
            if (i > Player.instance.maxHealth - 1)
            {
                hearts[i].gameObject.SetActive(false);
            }
            else
            {
                hearts[i].gameObject.SetActive(true);
                if (i + 1 <= Player.instance.health)
                {
                    hearts[i].sprite = heart;
                }
                else
                {
                    if (i <= Player.instance.health && Player.instance.health % 1 > 0.45f)
                    {
                        hearts[i].sprite = halfHeart;
                    }
                    else
                    {
                        hearts[i].sprite = empty;
                    }
                }
            }
        }
    }

    private void SetShield()
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i > Player.instance.maxHealth - 1 || Player.instance.shield == 0)
            {
                shields[i].gameObject.SetActive(false);
            }
            else
            {
                if (i <= Player.instance.shield)
                {
                    shields[i].gameObject.SetActive(true);

                    var ratio = Player.instance.shield % 1;
                    if (ratio != 0 && i == (int)Mathf.Ceil(Player.instance.shield - 1))
                    {
                        shields[i].color = new Color(1, 1, 1, ratio);
                    }
                    else
                    {
                        shields[i].color = Color.white;
                    }
                }
                else
                {
                    shields[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
