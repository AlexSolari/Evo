﻿using Evo.Core.Entities;
using Evo.Core.Interfaces;
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
        public static Dictionary<ILifeForm, Dictionary<ILifeForm, double>> SquaredDistances = new Dictionary<ILifeForm, Dictionary<ILifeForm, double>>();
        public static Config SystemConfig;
        public static int Width { get { return SystemConfig.Width; } }
        public static int Height { get { return SystemConfig.Height; } }

        public const int ChargeDistance = 200;
        public const int ChargeSpeedDelta = 2;
        public static List<ILifeForm> Herbivores = new List<ILifeForm>();
        public static List<ILifeForm> Predators = new List<ILifeForm>();
        public static List<Cell> Cells = new List<Cell>();
        public static int HerbivoresLimit = 400;
        public static int CellsLimit = 500;
        internal static List<Action> PendingActions = new List<Action>();

        public static int AITickDelay {
            get 
            {
                return 5;//(int)Math.Pow(1.73, Global.Objects.Count / 5); 
            } 
        }

        public static void ReduceVector(ref Vector2 vector, float limit, float epsilon = 0.1f)
        {
            while (vector.LengthSquared() > (limit + epsilon) * (limit + epsilon))
            {
                vector.X *= 0.9f;
                vector.Y *= 0.9f;
            }
        }

        public static bool IsNear(float x1, float x2)
        {
            return Math.Abs(x1 - x2) < 10;
        }

        public static double DistanceSquared(Cell a, Cell b)
        {
            if (a == null || b == null)
                return 0;
            return (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);
        }

        public static Point CreateRandomPoint()
        {
            return new Point(Rand.Int(Global.Width), Rand.Int(Global.Height));
        }
    }
}
