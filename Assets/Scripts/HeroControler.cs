using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;


public class HeroControler : MonoBehaviour {

    private Rigidbody2D rigidbody_;
    private Animator animator_;
    private SpriteRenderer sprite_;
    private BoxCollider2D box_collider_;
    private CircleCollider2D circle_collider_;
    private EdgeCollider2D edge_collider_;

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
        box_collider_ = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        TimersTick();
    }

    void Update()
    {
        State = HeroState.Idle1;
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

    int last_direction_ = 1;
    private void Move()
    {
        
           Vector3 direction = transform.right * Input.GetAxis("Horizontal");
           direction *= last_direction_;
           if (direction.x < 0 && last_direction_ == 1 )
           {
               transform.rotation = Quaternion.Euler(0, 180, 0);
               last_direction_ = -1;
           }
           else if(direction.x > 0 && last_direction_ == -1)
           {
               transform.rotation = Quaternion.Euler(0, 0, 0);
               last_direction_ = 1;
           }

           transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed_ * Time.deltaTime);

           State = HeroState.Run;

    }

    private void Jump()
    {
        rigidbody_.AddForce(transform.up * jump_force_, ForceMode2D.Impulse);
    }

    private int attack_timer_ = 0;
    private int last_attack_;
    private void Attack()
    {
        if (last_attack_ == 1 && attack_timer_ > 0)
        {
            State = HeroState.Attack2;
            last_attack_ = 2;
        }
        else if(last_attack_ == 2 && attack_timer_ > 0)
        {
            State = HeroState.Attack3;
            last_attack_ = 3;
        }
        else
        {
            State = HeroState.Attack1;
            last_attack_ = 1;
        }
        attack_timer_ = 45;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position - new Vector3(0, 1.15f), new Vector2(1, 0.2f), 0);

        is_grounded_ = colliders.Length > 1;
        
    }

    private void TimersTick()
    {
        if(attack_timer_ > 0)
        {
            --attack_timer_;
        }
    }
}
