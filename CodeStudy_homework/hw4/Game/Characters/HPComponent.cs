using hw4.Engine.Component;

namespace hw4.Game.Characters
{
    public class HPComponent : ComponentBehaviour
    {
        private int _hp = 0;

        public void SetHP(int hp)
        {
            _hp = hp;
        }

        public void TakeDamage(int damage)
        {
            _hp -= damage;
            if (_hp < 0)
            {
                _hp = 0;
            }
        }
        public int GetHP()
        {
            return _hp;
        }

        public bool IsDead()
        {
            return _hp <= 0;
        }
    }
}
