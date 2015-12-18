using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    private Vector2 _lastPlayerPosition;
		
    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        _lastPlayerPosition = Player.instance.transform.position;
        while (Mathf.Abs(_lastPlayerPosition.x - Player.instance.transform.position.x) < 1)
        {
            yield return null;
        }
        
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        var timer = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            foreach (var s in spriteRenderers)
            {
                s.color = Color.Lerp(Color.white, Color.clear, timer / 2);
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}
