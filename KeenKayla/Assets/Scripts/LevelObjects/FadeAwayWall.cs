using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class FadeAwayWall : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    public void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        if(renderers.Length == 0)
        {
            Debug.LogError("FadeAwayWall " + gameObject.name + " " + gameObject.GetInstanceID() + " has no sprite renderers");
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {        
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        var currentAlpha = renderers[0].color.a;
        while (currentAlpha > 0)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, 0, Time.deltaTime);
            foreach (var s in renderers)
            {
                var color = s.color;
                color.a = currentAlpha;
                s.color = color;
            }
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        var currentAlpha = renderers[0].color.a;
        while (currentAlpha < 1)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, 1, Time.deltaTime);
            foreach (var s in renderers)
            {
                var color = s.color;
                color.a = currentAlpha;
                s.color = color;
            }
            yield return null;
        }
    }
}
