using Evo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Interfaces
{
    public interface IHerbivore
    {
        int GrowTimer { get; set; }
        void ResetGrowTimer();
        void Runaway(Predator from, int chilloutTimer = 500);
        void Chill();
    }
}
