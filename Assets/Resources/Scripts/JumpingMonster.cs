using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JumpingMonster : Unit  
{
    [SerializeField]
    private float leftPoint;
    [SerializeField]
    private float rightPoint;
    [SerializeField]
    private float jumpForce = 10.0F;
    [SerializeField]
    private float jumpLenght = 10.0F;
    [SerializeField]
    private LayerMask Ground;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D coll;
    private Animator anim;

    private bool facingLeft = true;

    protected override void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (anim.GetBool("Jump"))
        {
            if(rb.velocity.y < .1)
            {
                anim.SetBool("Fall", true);
                anim.SetBool("Jump", false);
            }
        }

        if(coll.IsTouchingLayers(Ground) && anim.GetBool("Fall"))
        {
            anim.SetBool("Fall", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Unit unit = other.gameObject.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 1.0F)
            {
                coll.enabled = false;
                rb.bodyType = RigidbodyType2D.Static;
                anim.SetTrigger("Death");
            }
            else unit.ReceiveDamage();  
        }
    }

    private void Jump()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftPoint)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector2(2, 2);
                }

                if (coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(-jumpLenght, jumpForce);
                    anim.SetBool("Jump", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightPoint)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector2(-2, 2);
                }

                if (coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(jumpLenght, jumpForce);
                    anim.SetBool("Jump", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }        
    }
}


