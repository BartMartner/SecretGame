using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossBar : MonoBehaviour
{
    public static BossBar instance;
    public Image innerBar;
    private float _lastValue;
    private Damagable _damagable;

    private void Awake()
    {
        instance = this;
        innerBar.type = Image.Type.Filled;
        innerBar.fillMethod = Image.FillMethod.Horizontal;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_damagable && _lastValue != _damagable.health)
        {
            _lastValue = _damagable.health;
            innerBar.fillAmount = _damagable.healthRatio;
        }
    }

    public void Show(Damagable damagable)
    {
        gameObject.SetActive(true);
        _damagable = damagable;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _damagable = null;
    }

    public void OnDestroy()
    {
        instance = null;
    }
}
