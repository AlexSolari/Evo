using Evo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Evo.Core.Debugging;

namespace Evo.Core.Entities
{
    class Predator : Cell, IPredator
    {
        public int ChaseTimer { get; set; }
        public int Hunger { get; set; }
        public Predator(Point position, int size = 4, double minspeed = 1, double maxspeed = 2)
            : base(position, size, minspeed, maxspeed, Color.Red)
        {
            Hunger = 500;
        }
        public override void Reproduce()
        {

            var rand = new Random();
            rand = new Random(Rand.Int());
            var pos = new Point((int)X + rand.Next(-10, 10), (int)Y + rand.Next(-10, 10));
            Scene.Add(new Predator(pos, 4, Global.GetRandomMinSpeed(typeof(Predator)), Global.GetRandomMaxSpeed(typeof(Predator))));
            Hunger = 500;
            base.Reproduce();
        }

        public void Eat(ILifeForm target)
        {
            Hunger += 200;
            Size += 1;
            Grow(1);//(int)Math.Ceiling((float)target.Size/Size));
            target.Die();
        }

        public override void AITick()
        {
            if (Hunger <= 0)
            { 
                Die();
                return;
            }
            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;
            var DistanceToTarget = (Math.Sqrt(Math.Pow(X - currentTarget.X, 2) + Math.Pow(Y - currentTarget.Y, 2)));

            var tmp = from cell in Global.Objects
                          where cell is Herbivore && cell.Size <= Size
                          select cell as Herbivore;

            
            foreach (var target in tmp)
            {
                double Distance = (Math.Sqrt(Math.Pow(X - target.X, 2) + Math.Pow(Y - target.Y, 2)));
                if (Distance <= Global.SystemConfig.TargetingRadius && Distance < DistanceToTarget)
                {
                    target.Chill();
                    ChaseTimer = (target.MaxSpeed >= MaxSpeed) ? 750 : 1500;
                    Target = target;
                    Speed = MaxSpeed;
                    target.Runaway(this);
                    break;
                }
            }

            ChaseTimer--;
            if (ChaseTimer <= 0)
            {
                Target = Global.CreateRandomPoint();
                Speed = MinSpeed;
            }

            base.AITick();
        }

        public override void Update()
        {
            if (AITickCounter == 0)
            {
                AITick();
                AITickCounter = Global.AITickDelay;
                Hunger--;
            }
            var nearestCells = (from cell in Global.Objects 
                               where (cell is Herbivore && (Math.Sqrt(Math.Pow(X - cell.X, 2) + Math.Pow(Y - cell.Y, 2))) < 5 && cell.Size <= Size) 
                               select cell).ToList();
            foreach (var item in nearestCells)
            {
                if (Target == item)
                {
                    Eat(item);
                    ChaseTimer = 0;
                }
                else if (Rand.Float() >= 0.5)
                    Eat(item);
            } 
            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;
            if ((int)currentTarget.X == (int)X && (int)currentTarget.Y == (int)Y)
                    {
                        Target = Global.CreateRandomPoint();
                        Speed = MinSpeed;
                    }
            base.Update();
        }
    }
}
