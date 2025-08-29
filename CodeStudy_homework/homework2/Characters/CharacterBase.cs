using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Characters
{
    public abstract class CharacterBase<TTargetManager,TTargetBase> : IDamage
    {
        protected Random _random;
        protected string _name;
        private int _hp;
        protected int _power;
        private bool _isDead;

        public CharacterBase(string name, int hp, int power)
        {
            _random = new Random();
            _name = name;
            _hp = hp;
            _power = power;
            _isDead = false;
        }

        public void TakeDamage(int rate)
        {
            if(_isDead)
            {
                return;
            }

            _hp -= rate;

            if (_hp<=0)
            {
                Console.WriteLine($"{_name} 은(는) {rate}만큼의 피해를 마지막으로 죽었다.");
                _isDead = true;
                _hp = 0;
            }
            else
            {
                Console.WriteLine($"{_name} 은(는) {rate} 의 피해를 입었다.\n남은 체력은 {_hp} 다.");
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


        public bool IsDead
        {
            get { return _isDead; }
        }

        protected abstract IDamage? SelectTarget(TTargetManager targetManager);

        protected abstract void Attack(IDamage? target);

        public virtual void Act(TTargetManager targetManager) { }


    }
}
