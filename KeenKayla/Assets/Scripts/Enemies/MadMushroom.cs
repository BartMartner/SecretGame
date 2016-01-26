using UnityEngine;
using System.Collections;

public class MadMushroom : Enemy
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Bounce());
    }

    public IEnumerator Bounce()
    {
        while (state == DamagableState.Alive)
        {
            for (int i = 0; i < 3; i++)
            {
                var bounceHeight = 1;
                if (i == 2)
                {
                    bounceHeight = 2;
                }

                var startingPosition = transform.position;
                var targetPosition = transform.position + Vector3.up * bounceHeight;

                var speed = 5;

                var delta = targetPosition - startingPosition;

                while (transform.position != targetPosition)
                {
                    var currentDelta = targetPosition - transform.position;
                    var speedMod = Mathf.Clamp(currentDelta.y / delta.y, 0.5f, 1);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * speedMod * Time.deltaTime);
                    yield return null;
                }

                while (transform.position != startingPosition)
                {
                    var currentDelta = transform.position - startingPosition;
                    var speedMod = Mathf.Clamp((delta.y-currentDelta.y) / delta.y, 0.65f, 1f);
                    transform.position = Vector3.MoveTowards(transform.position, startingPosition, speed * speedMod * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
