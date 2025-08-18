using HomeworkGame.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Characters
{
    public abstract class UnitBase : CharacterBase
    {

        protected int _attackCount;
        protected int _killCount;
        protected eUnitTypes _type;

        public UnitBase(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
        }

        public virtual void ShowSelectMessage()
        {
            Console.WriteLine("공격할 대상의 번호를 입력하세요. : ");
        }

        public int GetAttackCount
        {
            get { return _attackCount; }
        }

        public int GetKillCount
        {
            get { return _killCount; }
        }

        public eUnitTypes GetUnitType
        {
            get { return _type; }
        }


    }
}
