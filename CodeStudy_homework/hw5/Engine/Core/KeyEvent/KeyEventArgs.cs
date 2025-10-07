
namespace hw4.Engine.Core.KeyEvent
{
    public struct KeyEventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; set; }
        public KeyEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }
}
