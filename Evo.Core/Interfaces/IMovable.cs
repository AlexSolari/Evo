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
        int Speed { get; set; }
        int MaxSpeed { get; set; }
        int MinSpeed { get; set; }
        Vector2 Direction { get; set; }
        void Move();
        void Rotate(float angle);
    }
}
