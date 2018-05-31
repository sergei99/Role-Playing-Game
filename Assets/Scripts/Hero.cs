using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    public enum CharacterGender { Male, Female };
    public CharacterGender Gender { get; }

    public enum CharacterRace { People, Dwarf, Elf, Orc, Goblin };
    public CharacterRace Race { get; }

    private uint experience = 0;
    public uint Experience { set; get; }

    public Hero(string Name, uint MaxHP = 0, uint Age = 0, CharacterGender Gender = CharacterGender.Male, CharacterRace Race = CharacterRace.People) : base(Name, MaxHP, Age)
    {
        this.Gender = Gender;
        this.Race = Race;
    }

    public override string ToString()
    {
        return base.ToString() + "; Character Gender: " + Gender.ToString() + "; Character Race: " + Race.ToString()
            + "; Experience: " + Experience;
    }
}