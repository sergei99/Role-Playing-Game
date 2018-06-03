using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterAI : MonoBehaviour {

    private Rigidbody2D rigidbody_;
    private Animator animator_;
    private SpriteRenderer sprite_;
    private BoxCollider2D collider_;

    private Vector3 start_position_;

    private GameObject Hero;

    public float agro_range = 5;
    public float attack_range = 2f;
    public int speed = 2;

    private delegate void  MonserBehaviour();
    MonserBehaviour MBehaviour;

    private System.Random randomizer = new System.Random();

    public enum MonsterState { Idle, Move, Attack};
    public MonsterState State
    {
        set
        {
            animator_.SetInteger("State", (int)value);
        }

        get
        {
            return (MonsterState)animator_.GetInteger("State");
        }
    }

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        sprite_ = GetComponent<SpriteRenderer>();
        collider_ = GetComponent<BoxCollider2D>();
        Hero = GameObject.FindWithTag("Player");
        start_position_ = transform.position;
        last_position_value_ = start_position_;
    }

    private void FixedUpdate()
    {
        CheckForPlayer();
    }

    void Update ()
    {
        State = MonsterState.Idle;
        MBehaviour();
	}

    private void MoveToTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Vector3 norm_direction = direction / direction.magnitude;

        MoveToNormDirection(norm_direction);
    }

    private void MoveToNormDirection(Vector3 norm_direction)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + norm_direction, speed * Time.deltaTime);

        sprite_.flipX = norm_direction.x > 0;
        State = MonsterState.Move;
    }

    private void Attack()
    {
        State = MonsterState.Attack;
    }

    private int norm_direction_ = 1;
    private Vector3 last_position_value_;
    private int last_position_counter_ = 0;
    private int timer_ = 120;
    private void Patrol()
    {
        --timer_;
        int changer = randomizer.Next(0, 100);
        if(changer == 1 && timer_ <= 0 || timer_ <= -1200)
        {
            changer = randomizer.Next(-1, 2);
            norm_direction_ = changer;
            timer_ = 120;
        }

        if(last_position_value_ == transform.position)
        {
            ++last_position_counter_;
            if(last_position_counter_ >= 3)
            {
                norm_direction_ = -norm_direction_;
            }
        }
        else
        {
            last_position_counter_ = 0;
        }

        if (norm_direction_ != 0)
        {
            Vector3 direction = Vector3.right * norm_direction_;
            MoveToNormDirection(direction);
        }
    }

    private void Track()
    {
        MoveToTarget(Hero.transform.position);
    }

    void CheckForPlayer()
    {
        Vector3 hero_distance = Hero.transform.position - transform.position;
        Vector3 current_distance = start_position_ - transform.position;

        float agro_range_sqr = agro_range * agro_range;

        if(hero_distance.sqrMagnitude <= attack_range * attack_range)
        {
            MBehaviour = Attack;
            Debug.Log("Attack");
        }
        else if (hero_distance.sqrMagnitude <= agro_range_sqr && current_distance.sqrMagnitude <= 2 * agro_range_sqr)
        {
            MBehaviour = Track;
        }
        else
        {
            MBehaviour = Patrol;
        }
    }


}
