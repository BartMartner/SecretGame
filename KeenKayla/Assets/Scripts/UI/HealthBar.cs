using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    public Sprite heart;
    public Sprite halfHeart;
    public Sprite empty;
    private Image[] hearts;
    private float _lastValue;
    private float _lastMaxValue;

    private void Awake()
    {
        hearts = GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        SetHearts();
    }

	// Update is called once per frame
	private void Update () 
    {
        if (_lastValue != Player.instance.health || _lastMaxValue != Player.instance.maxHealth)
        {
            _lastValue = Player.instance.health;
            _lastMaxValue = Player.instance.maxHealth;
            SetHearts();
        }
	}

    private void SetHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i > Player.instance.maxHealth - 1)
            {
                hearts[i].gameObject.SetActive(false);
            }
            else
            {
                hearts[i].gameObject.SetActive(true);
                if(i+1 <= Player.instance.health)
                {
                    hearts[i].sprite = heart;
                }
                else
                {
                    if(i <= Player.instance.health && Player.instance.health % 1 > 0.45f)
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
}
