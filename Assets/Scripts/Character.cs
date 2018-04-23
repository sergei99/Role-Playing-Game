using System.Collections;
using System.Collections.Generic;



namespace GameCore
{
    public abstract class Character
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

        public Character(string Name, uint MaxHP = 0, uint Age = 0)
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

    public class Enemy : Character
    {

        public Enemy(string Name, uint MaxHP = 0) : base(Name, MaxHP)
        {

        }
    }

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
}






