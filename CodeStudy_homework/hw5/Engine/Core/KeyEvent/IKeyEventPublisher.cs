using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Engine.Core.KeyEvent
{
    public interface IKeyEventPublisher
    {
        public void Subscribe(KeyPressedDelegate func);
        public void Unsubscribe(KeyPressedDelegate func);
    }
}
