using Evo.Core.Entities;
using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core
{
    public class Global
    {
        public static int Width = 500;
        public static int Height = 500;
        public static List<Cell> Objects = new List<Cell>();
        public static int GrowLimit = 10;
        public static int TargetingRadius = 200;

        public static void ReduceVector(ref Vector2 vector, float limit)
        {
            var EPSILON = 0.001f;
            while (vector.Length > limit + EPSILON)
            {
                vector.X *= 0.9f;
                vector.Y *= 0.9f;
            }
            if (Math.Abs(vector.X) < 0.1f) vector.X = 0;
            if (Math.Abs(vector.Y) < 0.1f) vector.Y = 0;
        }

        public static bool IsNear(float x1, float x2)
        {
            return Math.Abs(x1 - x2) < 10;
        }
    }
}
