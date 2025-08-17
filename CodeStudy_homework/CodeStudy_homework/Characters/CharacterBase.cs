using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Characters
{
    public abstract class CharacterBase
    {
        private string _name;
        private int _hp;
        private int _power;
        private bool _isDefense;
        private bool _isDead;

        public CharacterBase(string name, int hp, int power)
        {
            _name = name;
            _hp = hp;
            _power = power;
            _isDefense = false;
            _isDead = false;
        }

        public void TakeDamage(int rate)
        {
            if(_isDead)
            {
                return;
            }

            _hp -= rate;

            Console.WriteLine($"{_name} 은(는) {rate} 의 피해를 입었다.\n남은 체력은 {_hp} 다.");

            if(_hp<=0)
            {
                Console.WriteLine($"{_name} 은(는) 죽었다.");
                _isDead = true;
            }
        }
        

        public string GetName
        {
            get { return _name; }
        }

        public int GetHp
        {
            get { return _hp; }
        }

        public int GetPower
        {
            get { return _power; }
        }

        public bool IsDefense
        {
            get { return _isDefense; }
        }

        public bool IsDead
        {
            get { return _isDead; }
        }



        public void SetDefense(bool option)
        {
            _isDefense = option;
        }


        public abstract void Attack(List<CharacterBase> targets);



    }
}
