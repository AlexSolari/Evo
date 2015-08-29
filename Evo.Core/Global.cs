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

        public const int ChargeDistance = 200;
        public const int ChargeSpeedDelta = 2;
        public static List<Cell> Objects = new List<Cell>();

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

        public static double Distance(dynamic a, dynamic b)
        {
            return (Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)));
        }

        public static Point CreateRandomPoint()
        {
            return new Point(Rand.Int(Global.Width), Rand.Int(Global.Height));
        }

        public static double GetRandomMaxSpeed(Type typeOfTarget)
        {
            var list = from cell in Objects
                                where cell.GetType() == typeOfTarget
                                select cell.MaxSpeed;
            double result = list.Sum() / list.Count();
            return Math.Ceiling(result + 1);
        }

        public static double GetRandomMinSpeed(Type typeOfTarget)
        {
            var list = from cell in Objects
                       where cell.GetType() == typeOfTarget
                       select cell.MinSpeed;
            double result = list.Sum() / list.Count();
            return (int)Math.Ceiling(result);
        }
    }
}
