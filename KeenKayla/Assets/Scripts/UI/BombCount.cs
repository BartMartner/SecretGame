using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombCount : MonoBehaviour
{
    private float _lastValue;
    private float _lastMaxValue;
    private Text _bombText;
    private Image _bombImage;

    private void Start()
    {
        _bombText = GetComponentInChildren<Text>();
        _bombImage = GetComponentInChildren<Image>();
        SetBombs();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_lastValue != Player.instance.currentBombs || _lastMaxValue != Player.instance.maxBombs)
        {
            _lastValue = Player.instance.health;
            _lastMaxValue = Player.instance.maxHealth;
            SetBombs();
        }
    }

    private void SetBombs()
    {
        if(Player.instance.maxBombs <= 0)
        {
            _bombImage.gameObject.SetActive(false);
            _bombText.gameObject.SetActive(false);
        }
        else
        {
            _bombImage.gameObject.SetActive(true);
            _bombText.gameObject.SetActive(true);
            _bombText.text = Player.instance.currentBombs.ToString();
        }
    }

}
