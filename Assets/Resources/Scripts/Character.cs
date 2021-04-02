using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private int lives = 5;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value <= 5)
            {
                lives = value;
                livesBar.Refresh();
            }
        }
        
    }
    private LivesBar livesBar;
    [SerializeField]
    private float speed = 3.0F;
    [SerializeField]
    private float jumpForce = 15.0F;
    [SerializeField]
    private LayerMask Ground;

    private bool isGrounded = false;

    private Bullet bullet;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int) value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;
    private Collider2D coll;

    protected override void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        bullet = Resources.Load<Bullet>("Bullet");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    protected override void Update()
    {
        if (State != CharState.Death)
        {
            if (State != CharState.Hurt)
            {
                if (isGrounded) State = CharState.Idle;

                if (isGrounded && Input.GetButtonDown("Fire1"))Shoot(); 
                if (Input.GetButton("Horizontal")) Run();
                if (isGrounded && Input.GetButtonDown("Jump")) Jump();
            }
        }
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        if (isGrounded) State = CharState.Run;
    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 0.125F; position.x += 0.25F;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
    }

    public override void ReceiveDamage()
    {
        Lives--;
        State = CharState.Hurt;
        if (Lives <= 0)
        {
            animator.SetTrigger("Death");
        }

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 5.0F, ForceMode2D.Impulse);

        Debug.Log(lives);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.6F);

        if (coll.IsTouchingLayers(Ground))
        {
            isGrounded = colliders.Length > 1;
        }

        if (!isGrounded) State = CharState.Jump;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }
}

public enum CharState
{
    Idle,
    Run,
    Jump,
    Hurt,
    Death
}
