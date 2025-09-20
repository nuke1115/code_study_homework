using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Game.Characters.CharacterManager
{
    public class MonsterManagerComponent : CharacterManagerComponentBase
    {
        public override void Start()
        {
            _opponentManager = ComponentOwner.GameObjectRequester.GetGameObject<GameObject>((int)eNames.UnitManager).GetComponent<UnitManagerComponent>();
        }

        public override void FillCharacters(int characterCnt)
        {
            for (int i = 0; i < characterCnt; i++)
            {
                switch (_random.Next(0, 3))
                {
                    case 0:
                        AddCharacter(_random.Next(20, 31), _random.Next(30, 41), "Oak", "지나가던 오!크");
                        break;
                    case 1:
                        AddCharacter(_random.Next(15, 26), _random.Next(30, 38), "Skeleton", "칼슘 보충제 먹는 스켈레톤");
                        break;
                    case 2:
                        AddCharacter(_random.Next(10, 25), _random.Next(20, 26), "Slime", "술라임");
                        break;
                }
            }
        }
        public override void Update()
        {
            if (GameContext.GameState == Game.GameContext.eGameStates.MONSTER_TURN)
            {
                var monster = _characterList[_random.Next(0, _characterList.GetAliveCount())];
                var unit = _opponentManager.GetCharacter();
                monster.attackerComponent.Attack(unit.HPComponent, unit.classComponent, _opponentManager.OnCharacterDeath);
                GameContext.GameState = Game.GameContext.eGameStates.UNIT_TURN;
            }
        }
    }
}
