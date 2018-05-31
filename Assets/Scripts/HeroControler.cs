using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControler : MonoBehaviour {

    private Rigidbody2D rigidbody_;
    private Animator animator_;
    private SpriteRenderer sprite_;
    private BoxCollider2D box_colider_;

    private int speed_ = 8;
    private int jump_force_ = 25;

    private bool is_grounded_ = false;

    private enum HeroState { Idle1, Idle2, Run, Attack1, Attack2, Attack3, Jump, Fall, Hurt, Die, SwordUp, SwordDown };
    private HeroState State
    {
        set
        {
            animator_.SetInteger("State", (int)value);
        }

        get
        {
            return (HeroState)animator_.GetInteger("State");
        }
    }

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        sprite_ = GetComponent<SpriteRenderer>();
        box_colider_ = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    void Update ()
    {
       // State = HeroState.Idle1;
        if (Input.GetButton("Horizontal"))
        {
            Move();
        }
        if(Input.GetButtonDown("Jump") && is_grounded_)
        {
            Jump();
        }
        
        if(Input.GetButtonDown("Attack1"))
        {
            Attack();
        }
        
    }

    private void Move()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed_ * Time.deltaTime);

        sprite_.flipX = direction.x < 0;
        State = HeroState.Run;
    }

    private void Jump()
    {
        rigidbody_.AddForce(transform.up * jump_force_, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        State = HeroState.Attack1;
    }

    private void CheckGround()
    {
        Collider2D[] coliders = Physics2D.OverlapBoxAll(transform.position - new Vector3(0, 1.15f), new Vector2(1, 0.2f), 0);

        is_grounded_ = coliders.Length > 1;
        
    }
}
