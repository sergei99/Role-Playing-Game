using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private static uint nextID = 0;

    public uint ID { get; }
    public string Name { get; }

    public uint Age { set; get; }

    public enum CharacterState { Normal, Weakened, Ill, Poisoned, Paralyzed, Dead };
    private CharacterState state = CharacterState.Normal;
    public CharacterState State { get { return state; } }

    public bool Voice { set; get; } = true;
    public bool Move { set; get; } = true;


    private uint hp = 0;
    public uint HP
    {
        set
        {
            if (value > maxHp)
            {
                hp = maxHp;
            }
            else
            {
                hp = value;
            }
            UpdateState();
        }

        get
        {
            return hp;
        }
    }

    private uint maxHp = 0;
    public uint MaxHP
    {
        set
        {
            if (value < hp)
            {
                hp = value;
            }
            maxHp = value;
        }

        get
        {
            return maxHp;
        }
    }

    protected Character(string Name, uint MaxHP = 0, uint Age = 0)
    {
        this.ID = nextID;
        this.Name = Name;
        this.hp = MaxHP;
        this.Age = Age;
        ++nextID;
    }

    private void UpdateState()
    {
        if (hp == 0)
        {
            state = CharacterState.Dead;
        }
        else if (hp < maxHp / 10 && state == CharacterState.Normal)
        {
            state = CharacterState.Weakened;
        }
        else if (hp >= maxHp / 10 && state == CharacterState.Weakened)
        {
            state = CharacterState.Normal;
        }
    }


    public override string ToString()
    {
        return "ID: " + ID.ToString() + "; Name: " + Name + "; Age " + Age + "; Character State: " + State.ToString() + "; Voice: " + Voice
            + "; Move: " + Move + "; Character Hit Points: " + hp + "; Character Max Hit Points: " + maxHp;
    }



}







