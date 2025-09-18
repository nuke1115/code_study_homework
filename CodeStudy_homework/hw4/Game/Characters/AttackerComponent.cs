using hw4.Engine.Component;

namespace hw4.Game.Characters
{
    public class AttackerComponent : ComponentBehaviour
    {
        private int _strength = 0;
        private int _killCnt = 0;
        private int _attackCnt = 0;

        public int Strength { get; set; }
        public int KillCnt { get; }
        public int AttackCnt { get; }

        public void Attack(HPComponent hPComponent)
        {
            if(hPComponent.IsDead())
            {
                return;
            }

            _attackCnt++;

            hPComponent.TakeDamage(_strength);

            if(hPComponent.IsDead())
            {
                _killCnt++;
            }
        }
    }
}
