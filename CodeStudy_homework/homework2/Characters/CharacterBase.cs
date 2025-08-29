
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

        public void TakeDamage(int rate, in string attacker)
        {
            if(_isDead)
            {
                return;
            }

            _hp -= rate;

            if (_hp<=0)
            {
                Console.WriteLine($"{attacker} -> {_name} : 피해 => {rate} => 사망");
                _isDead = true;
                _hp = 0;
            }
            else
            {
                Console.WriteLine($"{attacker} -> {_name} : 피해 => {rate} => {_hp}");
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

        protected virtual void Attack(IDamage? target)
        {
            if (target is null || target.IsDead)
            {
                return;
            }

            target.TakeDamage(_power, _name);
        }

        public virtual void Act(TTargetManager targetManager) { }


    }
}
