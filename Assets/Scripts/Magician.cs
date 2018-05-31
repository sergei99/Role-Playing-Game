using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : Hero
{
    private uint mp;
    public uint MP
    {
        set
        {
            if (value > maxMp)
            {
                mp = maxMp;
            }
            else
            {
                mp = value;
            }
        }

        get
        {
            return mp;
        }
    }

    private uint maxMp;
    public uint MaxMP
    {
        set
        {

            if (value < mp)
            {
                mp = value;
            }
            maxMp = value;
        }

        get
        {
            return maxMp;
        }
    }


    public Magician(string Name, uint MaxHP = 0, uint Age = 0, uint MaxMP = 0, CharacterGender Gender = CharacterGender.Male,
        CharacterRace Race = CharacterRace.People) : base(Name, MaxHP, Age, Gender, Race)
    {
        this.mp = MaxMP;
    }

    public override string ToString()
    {
        return base.ToString() + "; Character Mana Points: " + mp + "; Character Max Mana Points: " + maxMp;
    }
}
