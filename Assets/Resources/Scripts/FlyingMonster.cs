using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : Unit
{
    [SerializeField]
    private float speed = 2.0F;
    [SerializeField]
    private float upPoint;
    [SerializeField]
    private float downPoint;

    private bool up = true;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        Fly();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.GetComponent<Bullet>();

        if (bullet)
        {
            coll.enabled = false;
            anim.SetTrigger("Death");
        }

        Character character = collider.GetComponent<Character>();

        if (character)
        {
            character.ReceiveDamage();
        }
    }

    private void Fly()
    {
        if (up == true)
        {
            rb.velocity = new Vector2(0f, speed);

            if (transform.position.y >= upPoint)
            {
                up = false;
            }
        }

        else
        {
            if (up == false)
            {
                rb.velocity = new Vector2(0f, -speed);

                if (transform.position.y <= downPoint)
                {
                    up = true;
                }
            }
        }
    }
}
