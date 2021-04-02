using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMonster : Unit
{
    [SerializeField]
    private float rate = 0.6F;

    private Vector3 direction;

    private Bullet bullet;
    private SpriteRenderer sprite;
    private Animator anim;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet2");
    }

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        direction = transform.right;

        InvokeRepeating("Shoot", rate, rate);
    }
    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 1.0F, -Vector2.right * direction.x);


        if(hit.collider != null)
        {
            anim.SetBool("Shoot", true);
            if (hit.collider.tag == "Player" && direction.x > 0.0F)
            {
                Vector3 position = transform.position; position.y += 0.25F;
                Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

                newBullet.Parent = gameObject;
                newBullet.Direction = -newBullet.transform.right;
                transform.localScale = new Vector2(-2, 2);
            }
            else
            {
                if(hit.collider.tag == "Player" && direction.x < 0.0F)
                {
                    Vector3 position = transform.position; position.y += 0.25F;
                    Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

                    newBullet.Parent = gameObject;
                    newBullet.Direction = newBullet.transform.right;
                    transform.localScale = new Vector2(2, 2);
                }
            }
        }
        else
        {
            anim.SetBool("Shoot", false);
            direction *= -1.0F;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 1.0F) ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }
}
