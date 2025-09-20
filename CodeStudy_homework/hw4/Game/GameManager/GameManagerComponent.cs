using hw4.Engine.Component;
using hw4.Engine.Core.KeyEvent;
using hw4.Game.Characters.CharacterManager;
namespace hw4.Game.GameManager
{
    public class GameManagerComponent : ComponentBehaviour
    {
        private GameContext.GameContext _ctx = new GameContext.GameContext();

        private bool _printed = false;
        private ConsoleKey _selectedKey = 0;
        private UnitManagerComponent _unitManager;
        private MonsterManagerComponent _monsterManager;

        public override void Awake()
        {
            ComponentOwner.LateUpdateEvent += LateUpdate;
            ComponentOwner.UpdateEvent += Update;
            ComponentOwner.KeyEventPublisher.Subscribe(GetKeyEvent);
            _ctx.GameState = GameContext.eGameStates.GAME_INITIAL_SCREEN;
        }

        public override void Start()
        {
            //컴포넌트 가져오기
            ComponentOwner.GameObjectRequester.GetGameObject<GameObject>((int)eNames.InputManager).GetComponent<InputManagerComponent>().Context = _ctx;
            _unitManager = ComponentOwner.GameObjectRequester.GetGameObject<GameObject>((int)eNames.UnitManager).GetComponent<UnitManagerComponent>();
            _monsterManager = ComponentOwner.GameObjectRequester.GetGameObject<GameObject>((int)eNames.MonsterManager).GetComponent<MonsterManagerComponent>();


        }

        public void GetKeyEvent(KeyEventArgs args)
        {
            _selectedKey = args.KeyInfo.Key;
        }

        public override void Update()
        {
            //게임 시작 코드
            if(_ctx.GameState != GameContext.eGameStates.GAME_INITIAL_SCREEN)
            {
                return;
            }

            if(!_printed)
            {
                Console.Clear();
                Console.WriteLine($"스페이스 또는 A키를 눌러 {_ctx.ElapsedGameCnt} 번째 게임 시작");
                Console.WriteLine("스페이스 : 일반 시작, A : 무한모드, ESC : 종료");
                _printed = true;
            }
            
            if(_selectedKey == ConsoleKey.Escape)
            {
                ComponentOwner.Terminator.Terminate();
                return;
            }

            _ctx.ResetLoopCnt();
            _printed = false;

            //게임 초기화 코드
            if (_selectedKey == ConsoleKey.Spacebar)
            {
                _ctx.SkipInfo = false;
                _ctx.MoveToNextGame();
                _ctx.GameState = GameContext.eGameStates.UNIT_TURN;
            }
            else if(_selectedKey == ConsoleKey.A || _ctx.SkipInfo)
            {
                _ctx.SkipInfo = true;
                _ctx.MoveToNextGame();
                _ctx.GameState = GameContext.eGameStates.UNIT_TURN;
            }

            _selectedKey = 0;

        }

        public override void LateUpdate()
        {
            //게임 끝 조건 체크
            if(_monsterManager.IsAllDead() || _unitManager.IsAllDead())
            {
                //_ctx.GameState = GameContext.eGameStates.GAME_END;//게임 종료 상태 처리하는거 짜기


                Console.WriteLine($"===== {_ctx.ElapsedGameCnt} 째 {_ctx.GameLoopCnt} 턴만에 게임 종료 =====");
                Console.WriteLine("==========");
                _unitManager.PrintCharacterStatus();
                Console.WriteLine("==========");
                _monsterManager.PrintCharacterStatus();
                Console.WriteLine("=====결과 : {0}=====", _unitManager.IsAllDead() ? "패배" : "승리");
                _ctx.GameState = GameContext.eGameStates.GAME_INITIAL_SCREEN;
            }
        }

        public override void OnDestroy()
        {
            ComponentOwner.LateUpdateEvent -= LateUpdate;
            ComponentOwner.UpdateEvent -= Update;
            ComponentOwner.KeyEventPublisher.Unsubscribe(GetKeyEvent);
        }

    }
}
