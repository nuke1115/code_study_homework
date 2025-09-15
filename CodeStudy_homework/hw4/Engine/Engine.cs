using hw3;
using hw4.Engine.Core;
using hw4.Engine.Core.KeyEvent;
using hw4.Engine.GameObject;
using System.Diagnostics;

namespace hw4.Engine
{
    public class Engine : IKeyEventPublisher, IGameObjectRequestable, ITerminatable
    {
        private MyArrayList<GameObjectBehaviour> _gameObjects = new MyArrayList<GameObjectBehaviour>(32);//자동으로 늘리게
        private event KeyPressedDelegate _keyPressedEvent;
        private bool _isRunning = true;
        private bool _gameObjectRemoveFlag = false;

        private double _fixedDeltaTimeInterval;
        private double _deltaTimeInterval;

        private MyArrayList<LifeCycleAction> _startEventList = new MyArrayList<LifeCycleAction>(32);
        private event LifeCycleAction _updateEvent;
        private event LifeCycleAction _fixedUpdateEvent;
        private event LifeCycleAction _lateUpdateEvent;


        public Engine(int targetFps)
        {
            _deltaTimeInterval = _fixedDeltaTimeInterval = 1000 / (double)targetFps;

        }


        public void Loop()
        {
            double deltaTime = 0;
            double fixedDeltaTime = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while(_isRunning)
            {
                watch.Restart();

                if(!_startEventList.IsEmpty())
                {
                    foreach(LifeCycleAction action in _startEventList)
                    {
                        action.Invoke();
                    }

                    _startEventList.Clear();
                }

                if(Console.KeyAvailable)
                {
                    _keyPressedEvent?.Invoke(new KeyEventArgs(Console.ReadKey(true)));
                }

                if(fixedDeltaTime >= _fixedDeltaTimeInterval)
                {
                    _fixedUpdateEvent?.Invoke();
                    fixedDeltaTime -= _fixedDeltaTimeInterval;
                }

                if (deltaTime >= _deltaTimeInterval)
                {
                    _updateEvent?.Invoke();
                    deltaTime = 0;
                }

                _lateUpdateEvent?.Invoke();

                if(_gameObjectRemoveFlag)
                {
                    _gameObjectRemoveFlag = false;
                    DestroyMarkedComponent();
                }    

                double time = watch.Elapsed.TotalMilliseconds;
                fixedDeltaTime += time;
                deltaTime += time;
            }

            DestroyEngine();

        }

        public void Subscribe(KeyPressedDelegate func)
        {
            _keyPressedEvent += func;
        }

        public void Unsubscribe(KeyPressedDelegate func)
        {
            _keyPressedEvent -= func;
        }


        public TGameObjectType Instantiate<TGameObjectType>() where TGameObjectType : GameObjectBehaviour, new()
        {
            if(_gameObjects.IsFull() && !_gameObjects.Resize(_gameObjects.GetCount() * 2))
            {
                throw new OutOfMemoryException("객체 최대 양 초과");
            }

            if(_startEventList.IsFull() && !_startEventList.Resize(_startEventList.GetCount()*2))
            {
                throw new OutOfMemoryException("객체 최대 양 초과");
            }

            TGameObjectType obj = new TGameObjectType();

            _fixedUpdateEvent += obj.FixedUpdate;
            _updateEvent += obj.Update;
            _lateUpdateEvent += obj.LateUpdate;
            obj.KeyEventPublisher = this;
            obj.GameObjectRequester = this;
            obj.Terminator = this;

            _gameObjects.Add(obj);

            obj.Awake();
            _startEventList.Add(obj.Start);

            return obj;
        }

        public TGameObjectType GetGameObject<TGameObjectType>(int objectNumber) where TGameObjectType : GameObjectBehaviour
        {
            foreach(var gameObject in _gameObjects)
            {
                if(gameObject.GameObjectNumber == objectNumber && gameObject is TGameObjectType)
                {
                    return (TGameObjectType)gameObject;
                }
            }

            return default(TGameObjectType);
        }

        [Obsolete("살릴지 말지 고민중")]
        public TGameObjectType[] GetGameObjects<TGameObjectType>(int objectNumber) where TGameObjectType : GameObjectBehaviour
        {
            throw new NotImplementedException();
        }

        private void DestroyMarkedComponent()
        {
            for (int i = _gameObjects.GetCount() - 1; i >= 0; i--)
            {
                if (_gameObjects[i].IsDestroyed)
                {
                    _gameObjects[i].OnDestroy();
                    _gameObjects.Remove(i);
                }
            }
        }

        public void DestroyDestroyedGameObjects()
        {
            _gameObjectRemoveFlag = true;
        }
        
        public void Terminate()
        {
            _isRunning = false;
        }

        private void DestroyEngine()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnDestroy();
            }

            _gameObjects.Clear();
        }
    }
}





////////////////////////////
//TEST
////////////////////////

public class Test
{

    public static void aMain()
    {
        Loop();
    }


    public static void Loop()
    {
        const double fdtc = 1000 / 10;
        const double dtc = 1000 / 10;
        double fdt = 0;
        double dt = 0;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {

            stopwatch.Restart();

            if (dt >= dtc)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"dt : {1 / dt * 1000}");
                dt = 0;

            }

            if (fdt >= fdtc)
            {
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"fdt : {1 / fdt * 1000}");
                fdt -= fdtc;
            }
            double time = stopwatch.Elapsed.TotalMilliseconds;

            fdt += time;
            dt += time;
        }
    }
}