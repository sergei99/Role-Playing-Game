using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    public enum MonsterState { idle, move, attack, hurt, die};
    private MonsterState state_;
    public MonsterState monster_state
    {
        get
        {
            return state_;
        }
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


}
