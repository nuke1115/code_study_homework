
namespace HomeworkGame.Characters
{
    public abstract class CharacterBase<TTargetManager,TTargetBase> : IDamage
    {
        protected Random _random;
        protected string _name;
        private int _hp;
        protected int _power;
        private bool _isDead;
        private readonly bool _printLog;

        public CharacterBase(string name, int hp, int power, bool printLog)
        {
            _random = new Random();
            _name = name;
            _hp = hp;
            _power = power;
            _isDead = false;
            _printLog = printLog;
        }

        public void TakeDamage(int rate, in string attacker)//근데, 어차피 한 게임이 끝나면 새로 생성되고, 출력 설정은 게임마다 바뀌는데, 생성자에서 출력 여ㅕ부 넣어주고, 여기서 그거에 따라 출력할지 말지 정할까?
        {
            if(_isDead)
            {
                return;
            }

            _hp -= rate;

            if (_hp<=0)
            {
                _isDead = true;
                _hp = 0;
                if(_printLog)
                {
                    Console.WriteLine($"{attacker} -> {_name} : 피해 => {rate}받고 사망");
                }
            }
            else if (_printLog)
            {
                Console.WriteLine($"{attacker} -> {_name} : 피해 => {rate}받고 체력 {_hp}됨");
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

        protected virtual void Attack(IDamage? target)//이걸 그냥, 다른 클래스로 만들고, 내부에 사용할 패턴을 정의한 그 클래스를 인스턴스로 가지게 할까? 그러면 갈아끼우기도 그냐마 간단해지는데. 보류
        {
            if (target is null || target.IsDead)
            {
                return;
            }

            target.TakeDamage(_power, _name);
        }

        public virtual void Act(TTargetManager targetManager) { }//제네릭 한정자로 개선 가능


    }
}
/*
근데, 굳이 IDamage에 모든걸 만들어둬야 할까?
일단, 지금 저렇게 해두고, 나중에 HP인터페이스 만들면, 그때 분리시키고, 그 인터페이스 상속시키자
 
 */