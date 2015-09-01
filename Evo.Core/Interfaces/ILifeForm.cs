using Evo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Interfaces
{
    public interface ILifeForm
    {
        int Age { get; set; }
        int GrowLimit { get; set; }
        void Die(Cell.DyingReason reason);
    }
}
