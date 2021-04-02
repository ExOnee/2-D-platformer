using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    private Animator anim;
    private Collider2D coll;

    protected override void Awake() { }
    protected override void Start() 
    {
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    protected override void Update() { }

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
}
