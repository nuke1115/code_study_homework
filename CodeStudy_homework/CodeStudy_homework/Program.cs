using HomeworkGame.Players.Player;
using HomeworkGame.Characters;
using HomeworkGame.DungeonPlace;
using HomeworkGame.GameStatus;
using System;
using HomeworkGame.Players.Monster;

namespace HomeworkGame
{
    public class MainProgram
    {
        GameContext _context;
        Player _player;
        Monster _monster;
        Dungeon _dungeon;
        Random _random;

        

        public static void Main()
        {
            MainProgram program = new MainProgram();


            program.InitGameProgram();

            if (program.RunGame() == false)
            {
                Console.WriteLine("게임이 비정상적으로 종료되었습니다");
                return;
            }

            Console.WriteLine("게임 종료");
        }


        private void InitGameProgram()
        {
            _context = new GameContext(3);
            _player = new Player();
            _monster = new Monster(_context);
            _dungeon = new Dungeon(_context);
            _random = new Random();
        }

        private bool RunGame()
        {
            while(_context.GetGameStatus != eGameStatus.GAME_TERMINATION)
            {
                if (_context.GetGameStatus == eGameStatus.INITIAL_SCREEN)
                {
                    Console.WriteLine("숙제-게임\n스페이스를 눌러 시작");
                    InitGameState();
                    ConsoleKey key = Console.ReadKey().Key;

                    if(key == ConsoleKey.Spacebar)
                    {
                        _context.SetGameStatus(eGameStatus.GAME_RUNNING);
                    }
                    else
                    {
                        _context.SetGameStatus(eGameStatus.GAME_TERMINATION);
                        break;
                    }

                    var coord = Console.GetCursorPosition();
                    coord.Left = 0;
                    Console.Clear();
                    Console.SetCursorPosition(coord.Left, coord.Top);

                }

                ProcessGameLogics();

                if (CheckGameEndCondition())
                {
                    EndGame();
                }
            }

            return true;
        }

        private void InitGameState()
        {
            _context.ResetContext(_random.Next(1, 5));
            _player.ResetCharacters();
            _monster.ResetCharacters();
        }

        private bool ProcessGameLogics()
        {
            bool ret = _dungeon.DoGameLogic(_player, _monster);
            Console.WriteLine("--------------------");
            return ret;
        }

        private bool CheckGameEndCondition()
        {
            return _player.IsAllDead() || _monster.IsAllDead();
        }

        private void EndGame()
        {

            Console.WriteLine("========== 게임 종료 ==========");
            foreach(UnitBase unit in _player.GetCharacterList())
            {
                Console.WriteLine("--------------------");
                Console.WriteLine($"{unit.GetName} ({unit.GetUnitType.ToString()})");
                Console.WriteLine($"공격 횟수 : {unit.GetAttackCount} / 처치 횟수 {unit.GetKillCount}");
                Console.Write($"체력 : {unit.GetHp} / 상태 : ");
                Console.WriteLine(unit.IsDead ? "사망" : "생존");
            }
            Console.WriteLine("----------------------");
            Console.Write($"결과 : ");
            Console.WriteLine(_player.IsAllDead() ? "패배" : "승리");
            Console.WriteLine("===========================");
            _context.SetGameStatus(eGameStatus.INITIAL_SCREEN);
        }
    }
}
/*
 
진입점 제작(Main)
클래스 6개 제작 (기사, 궁수, 마법사), (슬라임,스켈레톤,오크)
스페이스 키 누르면 리스트에 몬스터 일정 마리수 넣기
스페이스 키 누르면 리스트에 유저 3개 넣기
던전 입장
몬스터-> 유저 (데미지, 남은체력 표기) , 유저 <- 몬스터(데미지, 남은체력 표기) 식으로 서로 데미지 가감
다 죽을때 까지 사이클
다 죽으면 클리어
결과 표기 (몇마리 잡고 죽었는지, 다 꺳는지)
다시 스페이스 초기화면으로


게임의 기본 흐름:
게임 초기화
(
게임 상태 구조체 초기화
던전 초기화(던전에 몹 삭제 및 생성)
플레이어 초기화(유닛 삭제 및 3개 생성)
게임 상태 구조체 설정
)

입력
(
게임 상태 확인
맞는 현제 상태에 따른 입력받기 함수 실행(아마 다형성 이용 가능할지도?)
)

게임 로직 처리
(
두 진영 중 하나가 다 죽었는지 확인
죽었다면 종료
아니라면 반복
유닛의 방어 상태를 해제한다
게임 상태 확인
입력값 확인 및 변환
올바른 입력값이라면 그에 맞는 함수 호출
공격받기 함수 호출

죽었다면, 몹의 경우는 리스트에서 삭제처리, 유닛은 비활성화(사망처리)
)
{
공격하기:
입력값이 유효 대상인 둘을 지정한다
두 값을 파싱
유닛이 공격한 횟수 1 올리기
대상 몹 공격

방어하기:
입력값이 유효 대상인 유닛 하나 지정
값 파싱
유닛의 상태를 방어로 바꾼다
}



게임 종료
(
결과 출력
유닛 리스트를 돌며 유닛의 정보를 출력
스페이스 눌러 처음 화면으로 돌아가게 하기
)


기본적으로, 게임 상태 구조체의 현제 상태에 따라 변하게 한다


루프 구조

루프(게임 실행중일동안)
(
게임 초기화(게임 시작 상태일때만)
입력(게임중 상태일때만)
게임 로직 실행(게임중 상태일때만)
게임 종료(게임 종료 상태일때만)
)

---------

게임의 흐름:

상태 표시:
유닛 선택
유닛에 따라 다른 선택 메시지 출력
입력받기
입력 검사
필요 시 추가로 입력 받기
행동하기
몬스터로 턴 넘기기
몬스터의 행동
게임종료조건 검사
유닛/몹 상태 초기화



방어 흐름:
유닛 선택->방어 선택

공격 흐름:
유닛 선택->공격 선택->필요에 따라 추가 입력


----------------

게임의 상태 클래스
(
게임 상태(enum bitmask로)
입력 상태
턴 수

게임 상태 알 수 있는 함수들
입력 상태 설정,가져오기 함수들
턴 수 설정,가져오기 함수들

)

게임 상태:
초기화면
정보 출력중
유닛 선택중
행동 선택중
대상 선택중
몬스터의 턴
종료화면

게임중인건 선택중, 몬스터의 턴 저것들중 하나라도 겹치는거 있으면 활성화

------------------

몹과 유닛들

몹과 유닛들의 공통적인 부분을 부모클래스로 만들고, 그것들을 각 클래스로 상속시켜서 구현하자

공통으로 가지는것:
체력, 공격력, 방어 여부, 공격 정보, 번호


-----------------







-----------

마법사/슬라임:
범위공격

궁수/스켈레톤:
방어뚫기

기사/오크:
그냥 순수 딜이 높음
 


------


왜 방어 뚫고 공격했다고 안뜸?
그리고 enum자체로 나오는거 수정
 */