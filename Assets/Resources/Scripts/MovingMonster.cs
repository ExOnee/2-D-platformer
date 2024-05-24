using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovingMonster : Unit
{
    [SerializeField]
    private float speed = 2.0F;

    private Vector3 direction;

    private SpriteRenderer sprite;

    private Animator anim;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Start()
    { 
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 1.0F)
            {
                anim.SetTrigger("Death");
            }
            else unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 1.0F + transform.right * direction.x * 1.1F, 0.05F);

        if ((colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>()))) direction *= -1.0F;

        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.5F + transform.right * direction.x * 1.1F, 0.05F);

        if ((colliders2.Length <= 0 && colliders.All(x => !x.GetComponent<Character>()))) direction *= -1.0F;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x > 0.0F;
    }
}
