using hw4.Engine.Component;
using hw4.Engine.Core.KeyEvent;

namespace hw4.Game.GameManager
{
    public class InputManagerComponent : ComponentBehaviour
    {
        public GameContext.GameContext Context { get; set; }

        public override void Awake()
        {
            ComponentOwner.KeyEventPublisher.Subscribe(GetInput);
            
        }

        public void GetInput(KeyEventArgs args)
        {
            if(Context is null)
            {
                return;
            }

            Context.PressedKeyInfo = args.KeyInfo;
        }

        public override void OnDestroy()
        {
            ComponentOwner.KeyEventPublisher.Unsubscribe(GetInput);
        }
    }
}
