using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Interfaces
{
    public interface IMovable
    {
        object Target { get; set; }
        double Speed { get; set; }
        double MaxSpeed { get; set; }
        double MinSpeed { get; set; }
        Vector2 Direction { get; set; }
        void Move();
    }
}
