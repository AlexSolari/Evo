using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Interfaces
{
    public interface IGrowable
    {
        int Age { get; set; }
        int GrowLimit { get; set; }
        int Size { get; set; }
        void Grow(int value);
    }
}
