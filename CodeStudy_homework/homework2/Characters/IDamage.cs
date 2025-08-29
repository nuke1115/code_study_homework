using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkGame.Characters
{
    public interface IDamage
    {
        public void TakeDamage(int rate);

        public bool IsDead { get; }
    }
}
