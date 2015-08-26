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
        public static Config SystemConfig;
        public static int Width { get { return SystemConfig.Width; } }
        public static int Height { get { return SystemConfig.Height; } }
        public static List<Cell> Objects = new List<Cell>();
        public static int GrowLimit = 10;

        public static int AITickDelay {
            get 
            {
                return 5;//(int)Math.Pow(1.73, Global.Objects.Count / 5); 
            } 
        }

        public static void ReduceVector(ref Vector2 vector, float limit, float epsilon = 0.1f)
        {
            while (vector.Length > limit + epsilon)
            {
                vector.X *= 0.9f;
                vector.Y *= 0.9f;
            }
            if (Math.Abs(vector.X) < epsilon) vector.X = 0;
            if (Math.Abs(vector.Y) < epsilon) vector.Y = 0;
        }

        public static bool IsNear(float x1, float x2)
        {
            return Math.Abs(x1 - x2) < 10;
        }

        public static Point CreateRandomPoint()
        {
            return new Point(Rand.Int(Global.Width-100)+50, Rand.Int(Global.Height-100)+50);
        }

        public static double GetRandomMaxSpeed(Type of)
        {
            List<double> list = null;
            if (of == typeof(Herbivore))
                list = (from cell in Objects where (cell is Herbivore) select cell.MaxSpeed).ToList();
            else
                list = (from cell in Objects where (cell is Predator) select cell.MaxSpeed).ToList();
            double result = (list.Sum() / list.Count);
            var coef = (Rand.Int(100) < 30) ? 1.5 : 0.5;
            return Math.Floor(result * coef);
        }

        public static double GetRandomMinSpeed(Type of)
        {
            List<double> list = null;
            if (of == typeof(Herbivore))
                list = (from cell in Objects where (cell is Herbivore) select cell.MinSpeed).ToList();
            else
                list = (from cell in Objects where (cell is Predator) select cell.MinSpeed).ToList();
            double result = (list.Sum() / list.Count);
            var coef = (Rand.Int(100) < 30) ? 1.5 : 0.5;
            return (int)Math.Floor(result * coef);
        }
    }
}
