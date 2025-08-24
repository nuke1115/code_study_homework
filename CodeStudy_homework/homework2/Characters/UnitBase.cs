using HomeworkGame.Players.Monster;

namespace HomeworkGame.Characters
{
    public abstract class UnitBase : CharacterBase<Monster,MonsterBase>
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
            Console.WriteLine("행동 : (1) : 공격, (2) : 방어, (정의되지 않은 입력) : 턴 넘기기");
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

        public override void Act(Monster type)
        {
            int act;
            if (int.TryParse(Console.ReadLine(), out act) == false)
            {
                act = -1;
            }

            if (act == 1)
            {
                Attack(SelectTarget(type));
            }
            else if (act == 2)
            {
                SetDefense(true);
            }
            else
            {
                Console.WriteLine($"불쌍한 {_name}은 턴을 넘기라는 지시를 받았습니다...");
            }
        }

        protected override List<MonsterBase>? SelectTarget(Monster monster)
        {
            Console.WriteLine("공격 할 대상의 인덱스를 지정하세요.\n선택 불가능한 대상이면 공격이 버려집니다.");

            MonsterBase? target;
            int selectedIndex;

            while (int.TryParse(Console.ReadLine(), out selectedIndex) == false) ;

            if (monster.TryGetCharacter(selectedIndex - 1, out target))
            {
                return new List<MonsterBase>() { target };
            }
            else
            {
                Console.WriteLine($"불쌍한 {_name}은 공격을 버리라는 지시를 받았습니다...");
                return null;
            }
        }
    }
}
