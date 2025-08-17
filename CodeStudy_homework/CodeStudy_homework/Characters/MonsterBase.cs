using HomeworkGame.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Characters
{
    public abstract class MonsterBase : CharacterBase
    {
        protected eMonsterTypes _type;
        public MonsterBase(string name, int hp, int power) : base(name, hp, power){ }

        public eMonsterTypes GetMonsterType
        {
            get { return _type; }
        }
    }
}
