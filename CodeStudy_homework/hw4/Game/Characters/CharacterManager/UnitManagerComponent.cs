using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Game.Characters.CharacterManager
{
    public class UnitManagerComponent : CharacterManagerComponentBase
    {
        public override void Start()
        {
            _opponentManager = ComponentOwner.GameObjectRequester.GetGameObject<GameObject>((int)eNames.MonsterManager).GetComponent<MonsterManagerComponent>();
        }

        public override void FillCharacters(int characterCnt)
        {

            for(int i = 0; i < characterCnt; i++)
            {
                switch(_random.Next(0,3))
                {
                    case 0:
                        AddCharacter(_random.Next(20, 31), _random.Next(30, 41), "Knight", "철(수저)기사");
                        break;
                    case 1:
                        AddCharacter(_random.Next(15, 26), _random.Next(30, 38), "Archer", "대한민국의 양궁 국가대표");
                        break;
                    case 2:
                        AddCharacter(_random.Next(10, 25), _random.Next(20, 26), "Magician", "흙흙마법사");
                        break;
                }
            }
        }
        public override void Update()
        {
            if(GameContext.GameState == Game.GameContext.eGameStates.UNIT_TURN)
            {
                GameContext.MoveToNextTurn();
                var unit = _characterList[_random.Next(0, _characterList.GetAliveCount())];
                var monster = _opponentManager.GetCharacter();
                unit.attackerComponent.Attack(monster.HPComponent, monster.classComponent, _opponentManager.OnCharacterDeath);
            }
        }
    }
}
