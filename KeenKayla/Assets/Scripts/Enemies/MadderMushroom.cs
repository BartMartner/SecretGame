using UnityEngine;
using System.Collections;

public class MadderMushroom : Enemy
{
    [HideInInspector]
    new public Rigidbody2D rigidbody2D;

    protected override void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        base.Start();
        StartCoroutine(JumpCheck());
    }

    private IEnumerator JumpCheck()
    {
        yield return new WaitForSeconds(Random.Range(0, 2f));

        bool shouldJump = false;
        while (state == DamagableState.Alive)
        {           
            var v = rigidbody2D.velocity;
            if (Vector3.Distance(Player.instance.transform.position, transform.position) < 6)
            {
                v.x = 5 * Mathf.Sign(Player.instance.transform.position.x - transform.position.x);
            }

            if (shouldJump)
            {
                v.y = 9;
            }
            else
            {
                v.y = 6;
            }

            rigidbody2D.velocity = v;

            var timer = 0f;

            while (groundedCheck.onGround && timer < 1)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            shouldJump = false;

            timer = 0;

            var delay = Random.Range(1, 2);

            while (timer < delay)
            {
                timer += Time.deltaTime;
                if (Player.instance.transform.position.y > transform.position.y + 1)
                {
                    shouldJump = true;
                }
                yield return null;
            }

            yield return null;
        }
    }
}
