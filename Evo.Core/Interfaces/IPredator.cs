using Evo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Interfaces
{
    public interface IPredator
    {
        bool TargetCaptured { get; set; }
        void Eat(Cell target);
        void UnlockTarget();
        void LockTarget(IHerbivore nearestHerbivore);
    }
}
