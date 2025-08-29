using HomeworkGame.Characters;
using HomeworkGame.Players.Monster;

namespace CodeStudy_homework_1.Characters.Units
{
    public class Wizard : UnitBase
    {
        public Wizard(string name, int hp, int power) : base(name, hp, power)
        {
            _attackCount = 0;
            _killCount = 0;
            _type = eUnitTypes.WIZARD;
        }

        public override void Act(Monster type)
        {
            foreach(IDamage? target in type.GetCharacterList())
            {
                Attack(target);
            }
        }
    }
}
