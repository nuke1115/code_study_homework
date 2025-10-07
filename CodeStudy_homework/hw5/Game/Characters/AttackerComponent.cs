using hw4.Engine.Component;

namespace hw4.Game.Characters
{
    public class AttackerComponent : ComponentBehaviour
    {
        
        private int _strength = 0;
        private int _killCnt = 0;
        private int _attackCnt = 0;
        private ClassComponent _myInfo;

        public int Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        public int KillCnt
        {
            get { return _killCnt; }
        }

        public int AttackCnt
        {
            get { return _attackCnt; }
        }
        public GameContext.GameContext GameContext { get;  set; }

        public override void Start()
        {
            _myInfo = ComponentOwner.GetComponent<ClassComponent>();
        }

        public void Attack(HPComponent HPComponent, ClassComponent classComponent, Action opponentCharacterDeathAction)
        {
            if(HPComponent.IsDead())
            {
                return;
            }

            _attackCnt++;

            HPComponent.TakeDamage(_strength);

            if(GameContext.SkipInfo == false)
            {
                Console.WriteLine($"{_myInfo.Name} 이 {classComponent.Name}을 공격해서 체력 {HPComponent.GetHP()}됨");
            }

            if(HPComponent.IsDead())
            {
                _killCnt++;

                if(GameContext.SkipInfo == false)
                {
                    Console.WriteLine($"{classComponent.Name}이 죽고, {_myInfo.Name}의 처치 횟수가 {_killCnt}로 올랐다");
                }

                opponentCharacterDeathAction?.Invoke();
            }
        }
    }
}
