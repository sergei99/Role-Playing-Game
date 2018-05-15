using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    public enum MonsterState { patrol, track};
    private MonsterState state_;
    public MonsterState Monster_state
    {
        get
        {
            return state_;
        }
    }

    private int direction = 1;

    Transform current;

    private void Patroling()
    {
        while(true)
        {
            transform.position.Set(transform.position.x + 1, transform.position.y, 0);
        }
    }

    private void Traking()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction *= -1;
    }

    // Use this for initialization
    void Start ()
    {
        current = GetComponent<Transform>();
        Patroling();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


}
