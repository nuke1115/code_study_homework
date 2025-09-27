using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.Generic;



using System;
using System.Threading;
using hw3;
using hw4.Engine;
using hw4.Engine.GameObject;
using hw4.Engine.Component;
using hw4.Engine.Core.KeyEvent;
using hw4.Game;
using hw4.Game.GameManager;
using hw4.Game.Characters.CharacterManager;
using hw4.Game.Characters;
using hw5;

/*

// 키 입력 이벤트 데이터를 담는 클래스
public class KeyPressEventArgs : EventArgs
{
    public ConsoleKeyInfo KeyInfo { get; }
    public DateTime PressTime { get; }

    public KeyPressEventArgs(ConsoleKeyInfo keyInfo)
    {
        KeyInfo = keyInfo;
        PressTime = DateTime.Now;
    }
}

// 이벤트를 발생시키는 클래스 (Publisher)
public class GameEngine
{
    // 키 입력 이벤트 선언
    public event EventHandler<KeyPressEventArgs> KeyPressed;

    private bool isRunning = true;

    // 게임 루프 시작
    public void StartGameLoop()
    {
        while (isRunning)
        {
            Console.WriteLine("프레임이 돕니다.");
            Thread.Sleep(1000);

            // 키 입력 체크 (논블로킹)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true = 키를 화면에 표시하지 않음
                OnKeyPressed(keyInfo);
            }
        }
    }

    // 이벤트 발생 메서드
    protected virtual void OnKeyPressed(ConsoleKeyInfo keyInfo)
    {
        KeyPressed?.Invoke(this, new KeyPressEventArgs(keyInfo));
    }

    public void Stop()
    {
        isRunning = false;
    }
}

// 이벤트를 구독하는 클래스들 (Subscribers)
public class InputHandler
{
    public void Subscribe(GameEngine engine)
    {
        //구독 - 링에 고리를 거는겁니다.
        engine.KeyPressed += OnKeyPressed;
    }

    private void OnKeyPressed(object sender, KeyPressEventArgs e)
    {
        Console.WriteLine($"\n[InputHandler] 키 입력됨: {e.KeyInfo.Key} (ASCII: {(int)e.KeyInfo.KeyChar}) at {e.PressTime:HH:mm:ss}");

        // 특정 키에 따른 동작
        switch (e.KeyInfo.Key)
        {
            case ConsoleKey.Escape:
                Console.WriteLine("게임을 종료합니다...");
                if (sender is GameEngine engine)
                    engine.Stop();
                break;
            case ConsoleKey.Spacebar:
                Console.WriteLine("점프!");
                break;
            case ConsoleKey.Enter:
                Console.WriteLine("액션!");
                break;
            default:
                Console.WriteLine($"'{e.KeyInfo.KeyChar}' 키가 눌렸습니다.");
                break;
        }
    }
}

public class ScoreManager
{
    private int score = 0;

    public void Subscribe(GameEngine engine)
    {
        engine.KeyPressed += OnKeyPressed;
    }

    private void OnKeyPressed(object sender, KeyPressEventArgs e)
    {
        // 숫자 키가 눌리면 점수 증가
        if (char.IsDigit(e.KeyInfo.KeyChar))
        {
            int points = int.Parse(e.KeyInfo.KeyChar.ToString());
            score += points;
            Console.WriteLine($"[ScoreManager] 점수 획득: +{points}, 총 점수: {score}");
        }
    }
}

class aProgram
{
    public static void ㅁㅁㅁMain()
    {
        Console.WriteLine("이벤트 기반 게임 시작!");
        Console.WriteLine("키를 눌러보세요. (ESC키로 종료)");
        Console.WriteLine("- 스페이스바: 점프");
        Console.WriteLine("- 엔터: 액션");
        Console.WriteLine("- 숫자키: 점수 획득");
        Console.WriteLine("=====================================\n");

        // 게임 엔진 생성
        var gameEngine = new GameEngine();

        // 이벤트 구독자들 생성 및 구독
        var inputHandler = new InputHandler();
        var scoreManager = new ScoreManager();

        inputHandler.Subscribe(gameEngine);
        scoreManager.Subscribe(gameEngine);

        // 게임 루프 시작 (블로킹)
        gameEngine.StartGameLoop();

        Console.WriteLine("게임이 종료되었습니다.");
    }
}


public interface IComponent
{
    GameObject Parent { get; set; }
}
public interface IGameControll
{
    void Update();
    void Start();
    void Awake();
}

public class GameObject
{
    private MyArrayList<IComponent> components = new MyArrayList<IComponent>(64);

    public T AddComponent<T>() where T : IComponent, new()
    {
        T component = new T();
        component.Parent = this;
        components.Add(component);
        return component;
    }

    public T GetComponent<T>() where T : IComponent, new()
    {
        foreach (var comp in components)
        {
            if (comp is T) return (T)comp;
        }
        return default(T);
    }
}
public class Transform : IComponent
{
    public GameObject Parent { get; set; }
    public int x;
    private int X { get; set; }
    public int y;
    public int Y { get; set; }
}
public class Renderer : IComponent
{
    public GameObject Parent { get; set; }
    public char Symbol { get; set; } = '*';
    public void Draw()
    {
        var pos = Parent.GetComponent<Transform>();
        Console.SetCursorPosition(pos.x, pos.y);
        Console.Write(Symbol);
    }
}
public class PlayerMove : IComponent
{
    public GameObject Parent { get; set; }

    public void Input()
    {
        if (!Console.KeyAvailable)
            return;

        var key = Console.ReadKey(true).Key;
        var pos = Parent.GetComponent<Transform>();

        switch (key)
        {
            case ConsoleKey.W: pos.y--; break;
            case ConsoleKey.S: pos.y++; break;
            case ConsoleKey.A: pos.x--; break;
            case ConsoleKey.D: pos.x++; break;
        }

    }

}

class Unity
{
    static void aMain()
    {
        Console.WriteLine("WASD로 움직이는 캐릭터 만들기");

        var Player = new GameObject();

        var pos = Player.AddComponent<Transform>();
        pos.x = 10;
        pos.y = 5;

        var render = Player.AddComponent<Renderer>();
        render.Symbol = '@';

        var move = Player.AddComponent<PlayerMove>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("WASD 움직이기");
            move.Input();
            render.Draw();
            Thread.Sleep(100);
        }


    }

}



class TestC<T> where T : new()
{

}

class Test2
{
    public Test2(int a) { }
    void f()
    {
    }
}




*/


/*
몬스터 던전 CMD 개발을 컴포넌트 식으로 바꾸기
Event 를 이용한 Update, Start,Awake등의 호출 만들기
유니티처럼 동작하기
GameObject에 Interface 다중으로 여러개도 붙일수 있다. 붙여서 플레이어, 적 생성하기
List< > 본인거 쓰세요
*/



/*
일단, Update, Awake같은 함수들을 정의할 인터페이스 1개

컴포넌트 저장하고 위 인터페이스 받아서 업데이트될 GameObject 1개

그리고, 그걸 상속받는 클래스 하나 만든다
그리고 그걸 컴포넌트들이 상속받는다

엔진이 루프마다 호출하는 게임 오브젝트의 업데이트 함수들은, 오브젝트 생성 시 자동으로 엔진에 바인딩 한다

키 입력은 직접 엔진에 바인드를 걸어둔다


초기 오브젝트 생성은 클래스를 만들고, 엔진이 그걸 호출하며 이루어진다


객체 생성은 엔진에 생성 함수 인터페이스 달고, 그걸 각 게임 오브젝트가 들고있는다.
그리고, 생성 시 컴포넌트는 자신의 소유 클래스로 가서 그 인터페이스를 호출함
삭제는 그 오브젝트를 삭제하면, 그걸 루프 종료때 일괄 검사해서 삭제한다


엔진은 루프마다 함수를 호출한다.


Instantiate는 빈 게임 오브젝트만 반환한다

------------

정의할것들

Instantiate, DestroyDestroyedGameObjects, FindGameObject, FindGameObjects를 정의하는 IGameObjectRequestable
함수들:
Instantiate : 제네릭으로 받은 게임 오브젝트를 생성 후 gameobjectList에 저장 후 리턴
DestroyDestroyedGameObjects : 삭제 요청 플레그 올린다(올려지면 메인 루프 마지막에 리스트 돌며 삭제될 오브젝트 삭제)
FindGameObject : int번호 입력받아 찾으면 오브젝트 리턴, 아니면 null리턴

BindToKeyEvent를 정의하는 IKeyEventBindable
BindToKeyEvent : 델리게이트 받아서 event에 저장시키는걸 의도

AddComponent,RemoveDestroyedComponents GetComponent, GetComponents를 정의하는 IComponentable

Awake,Start,FixedUpdate,Update,LateUpdate,OnDestroy를 정의하는 ILifeCyclable


모든 컴포넌트에 들어가야 되는것들을 정의하는 ComponentBehaviour : ILifeCyclable
들어가야 될 것:
destroy플레그, 프로퍼티
주인 오브젝트 GameObjectBehaviour 프로퍼티
함수들:
virtual로 기본형만 선언된 업데이트 함수들
Destroy함수 : 자신의 플레그 올리고, RemoveDestroyedComponents 호출



모든 게임오브젝트에 들어가야 되는것들을 정의하는 GameObjectBehaviour : ILifeCyclable, IComponentable
들어가야 될 것:
컴포넌트 저장 리스트
destroy플레그, 프로퍼티
componentDestroy플레그
int번호{프로퍼티,get;set;}
게임오브젝트리퀘스터{프로퍼티,get;set;}
키바인더{프로퍼티,get;set;}
각 라이프사이클 이벤트들
함수:
Destroy함수 : 자신의 플레그 올리고, DestroyDestroyedGameObjects 함수 호출
각 라이프사이클 이벤트 구독 및 해제 함수들(컴포넌트가 호출할거임)

engine : IGameObjectRequestable, IKeyBindable
들어가야 될 것:
GameObject리스트
objectDestroy플레그
각 라이프사이클과 키 이벤트들
함수:
각 라이프사이클 이벤트 구독 및 해제 함수들(게임오브젝트가 호출할거임)
메인루프
그리고 더 쓸게 있긴 한데, 이건 작성하면서 생각하고

*/

/*
오브젝트 생성 과정
클래스 타입 넣어서 엔진에 호출(또는 인터페이스로 호출)=>엔진에 요청 들어감=>생성=>관리 리스트에 등록=>
업데이트 이벤트에 등록=>요청 관련 인터페이스 주입=>Awake,Start 순으로 호출

오브젝트 삭제 과정
Destroy호출 => destroy플레그 킴 => 엔진에 삭제 요청 => 엔진이 삭제 플레그 올림 => 루프 끝날 때 리스트 돌며 삭제 함수 호출
=> 컴포넌트 리스트 돌며 OnDestroy호출 => 엔진이 업데이트 이벤트들에서 함수 제거

컴포넌트 부착 과정
오브젝트에 요청=>생성=>Awake,Start순으로 호출=>컴포넌트 리스트에 등록

컴포넌트 삭제 과정
destroy플레그 킴 => 오브젝트에 삭제 요청 => 오브젝트가 삭제 플레그 올림 => 컴포넌트 리스트 순회하며 마크된거 OnDestroy 호출=>리스트에서 제거

라이프사이클 업데이트 함수 호출 과정
이벤트 호출=>오브젝트가 자기의 업데이트 이벤트 호출

키 입력
초기:엔진의 키 업데이트 이벤트에 자기 자신 등록
키 입력 감지됨=>이벤트 호출

*/
/*
 컴포넌트에서 바인딩을 비워둔 이유 : 개발자가 원하는 라이프사이클에만 바인딩 걸도록 해주기 위해 => 성능문제
GO에서 Awake, Start만 virtual로 열어둔 이유:Awake, Start에서 어떤 컴포넌트를 가질지 정해두고, 그떄 초기 컴포넌트들 붙여두라고
개발자가 이벤트 바인딩,언바인딩 하게 함=>일관성 유지하게 하려고 결정
(유저가 할당한건 유저가, 엔진이 할당해준것은 엔진이 책임지고 해제)
 */


public class TestOOOO
{

}


public class Program
{
    public static bool Test1()
    {
        Console.WriteLine("1");
        return true;
    }

    public static bool Test2()
    {
        Console.WriteLine("2");
        return true;
    }
    public static void Main()
    {
        Program p = new Program();
        
        int sel = int.Parse(Console.ReadLine());

        if (sel == 1) p.RunGame();
        else if(sel == 2) p.RunCodeGen();
        
    }

    public void RunCodeGen()
    {
        CodeGenerator codeGenerator = new CodeGenerator();

    }

    public void RunGame()
    {
        Engine engine = new Engine(240);
        GameObject go = engine.Instantiate<GameObject>();
        go.AddComponent<UnitManagerComponent>();
        go.GameObjectNumber = (int)eNames.UnitManager;

        go = engine.Instantiate<GameObject>();
        go.AddComponent<MonsterManagerComponent>();
        go.GameObjectNumber = (int)eNames.MonsterManager;


        go = engine.Instantiate<GameObject>();
        go.AddComponent<InputManagerComponent>();
        go.GameObjectNumber = (int)eNames.InputManager;

        go = engine.Instantiate<GameObject>();
        go.GameObjectNumber = (int)eNames.GameManager;
        go.AddComponent<GameManagerComponent>();
        
        

        engine.Loop();
    }
}
/*
 호출 타이밍 꼬임 문제

        

        
 */


//아 컴포넌트들에 심각한 버그 있다 이거 하고 빨리 고치자
//야! 근데 어차피 리스트에서 Destory호출하고, 그 프레임에 바로 게임오브젝트 다시 생성해도되지않냐? 어차피 게임엔진은 마크된것만 지울거잖아
//근데, 게임 종료쪽에서 클리어만 호출하게 하면 되겠네
//아직 GameManager할거 많음
//이제 게임종료쪽을 어떻게 처리하냐?
/*



입력 대기
게임 기물 초기화
던전 호출
게임 정보 출력
유닛 매니저에서 유닛 선택
유닛 매니저에서 유닛 가져오기
유닛 매니저에서 유닛 행동시키기
몬스터 매니저에서 동일 과정
종료 조건 검사
종료



==>


게임 시작일때:
스킵이 On이면 바로 유닛턴으로
아니라면, 키 이벤트가 호출될때까지 그대로 넘기기
호출된 후, 어떤 키냐에 따라 스킵을 켤지, 끌지 정한다

유닛 턴일때:
유닛이 몬스터 메니저에서 몬스터 하나 고른 후 공격
그리고 몬스터 턴으로

몬스터 턴일때:
몬스터가 유닛 메니저에서 몬스터 하나 고른 후 공격
그리고 유닛 턴으로

게임 종료일때:
결과 출력
게임 시작 상태로 넘기기

LateUpdate에서:
게임 종료 조건 검사
종료조건이 맞다면, 게임 종료 상태 키기

*/
/*

HP컴포넌트 있고 <- 체력 설정, 체력 가감, 죽었는지 플레그 있고
Attacker컴포넌트 있고 <- 대상 컴포넌트 받아서, 거기로 공격 넣는것, 공격력, 죽인 횟수, 공격 횟수
Class 컴포넌트 있고 <- 어떤 클래스인지, 이름은 뭔지

캐릭터 매니저에서는 저것들의 묶음을 들고있는다
또, 캐릭터 매니저에서는 저걸 그냥 드는게 아닌, 래퍼클래스를 쓴다.(밑에 있는 구상으로)


게임의 진행을 관리할 GameManager <= 게임의 시작과 종료, 그리고 컨텍스트 초기화 담당

게임의 상태를 관리할 GameCOntext

입력을 받고, 컨텍스트에 받은 키 올리고, 스킵 여부 설정 할 입력 매니저

유닛들을 가지고 있을 유닛 매니저
몬스터들을 가지고 있을 몬스터 매니저

유닛들
몬스터들

게임의 상태를 정의 할 enum상수들



----

캐릭터 리스트에서 죽은 유닛이 발생하면, 산것들의 가장 마지막 요소하고 위치를 바꾸고 거기를 가리키던 포인터를 1 줄인다
그리고, 0~인덱스 범위에서 랜덤으로 뽑으면, 랜덤 여러번 안쓰고 한번에 딱 뽑을 수 있음

아니 이거 도움 준 아이디어인데 생각보다 좋음


*/