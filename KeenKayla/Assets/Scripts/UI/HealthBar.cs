using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
    public Image innerBar;
    private float _lastValue;
	
    private void Awake()
    {
        innerBar.type = Image.Type.Filled;
        innerBar.fillMethod = Image.FillMethod.Horizontal;
    }

	// Update is called once per frame
	private void Update () 
    {
        if (_lastValue != Player.instance.health)
        {
            _lastValue = Player.instance.health;
            innerBar.fillAmount = Player.instance.healthRatio;
        }
	}
}
