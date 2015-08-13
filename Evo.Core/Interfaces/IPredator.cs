using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Interfaces
{
    public interface IPredator
    {
        void Eat(ILifeForm target);
    }
}
