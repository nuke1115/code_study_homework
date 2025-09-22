using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4.Engine.Core
{
    public interface ILifeCyclable
    {
        public void Awake();
        public void Start();
        public void Update();
        public void FixedUpdate();
        public void LateUpdate();
        public void OnDestroy();
    }
}
