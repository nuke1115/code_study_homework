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

        private int _attackCount;
        private int _killCount;
        private eUnitTypes _type;

        public UnitBase(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
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
