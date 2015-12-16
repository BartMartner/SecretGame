using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour 
{
    public static LightManager instance;
    public Light mainLight;
    public Light deathLight;
    public Light fillLight;

	private void Awake () 
    {
        instance = this;
	}

    private void OnDestroy()
    {
        instance = null;
    }

    public IEnumerator DeathLight()
    {
        deathLight.gameObject.SetActive(true);

        var timer = 0f;
        while (timer < 5f)
        {
            timer += Time.deltaTime;

            if(mainLight.intensity > 0)
            {
                mainLight.intensity -= Time.deltaTime;
            }

            deathLight.color = Color.Lerp(Color.cyan, new Color(0,0.2f,0.4f), timer / 5f);
            deathLight.intensity = timer;

            yield return null;
        }
    }
}
