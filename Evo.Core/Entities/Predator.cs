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
        public Predator(Point position, int size = 2, double minspeed = 1, double maxspeed = 3)
            : base(position, size, minspeed, maxspeed, Color.Red)
        {
            Hunger = 400;
        }
        public override void Reproduce()
        {

            var rand = new Random();
            for (int i = 2; i > 0; i--)
            {
                rand = new Random(Rand.Int());
                var pos = new Point((int)X + rand.Next(-10, 10), (int)Y + rand.Next(-10, 10));
                Scene.Add(new Predator(pos, 2, Global.GetRandomMinSpeed(typeof(Predator)), Global.GetRandomMaxSpeed(typeof(Predator))));
            }

            Hunger = 400;
        }

        public void Eat(ILifeForm target)
        {
            Hunger += 100;
            Grow(2);
            target.Die();
        }

        public override void AITick()
        {
            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;
            var DistanceToTarget = (Math.Sqrt(Math.Pow(X - currentTarget.X, 2) + Math.Pow(Y - currentTarget.Y, 2)));

            var tmp = Global.Objects.ToList();

            if (Hunger <= 0)
                Die();
            foreach (Cell target in tmp)
            {
                if (target is Herbivore)
                {
                    var convertedTarget = target as Herbivore;
                    double Distance = (Math.Sqrt(Math.Pow(X - convertedTarget.X, 2) + Math.Pow(Y - convertedTarget.Y, 2)));
                    if (Distance <= Global.SystemConfig.TargetingRadius && Distance < DistanceToTarget)
                    {
                        convertedTarget.Chill();
                        ChaseTimer = (convertedTarget.MaxSpeed >= MaxSpeed) ? 750 : 1500;
                        Target = target;
                        Speed = MaxSpeed;
                        convertedTarget.Runaway(this);
                        break;
                    }
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
            }
            var tmp = Global.Objects.ToList();
            var nearestCells = from cell in tmp where (cell is Herbivore && (Math.Sqrt(Math.Pow(X - cell.X, 2) + Math.Pow(Y - cell.Y, 2))) < 5) select cell;
            foreach (var item in nearestCells)
            {

                if (Target == item)
                {
                    Eat(item as ILifeForm);
                    ChaseTimer = 0;
                }
                else if (Rand.Float() >= 0.5)
                    Eat(item as ILifeForm);
            }
            Hunger--;
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
