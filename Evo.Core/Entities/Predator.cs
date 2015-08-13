using Evo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Evo.Core.Entities
{
    class Predator : Cell, IPredator
    {
        public int ChaseTimer { get; set; }
        public int Hunger { get; set; }
        public Predator(Point position, int size = 2, int minspeed = 1, int maxspeed = 3) : base(position, size, minspeed, maxspeed, Color.Red)
        {
            Hunger = 500;
        }
        public override void Reproduce()
        {
            var rand = new Random();

            for (int i = rand.Next(2, 3); i != 0; i--)
            {
                var pos = new Point((int)X + rand.Next(-10, 10), (int)Y + rand.Next(-10, 10));
                Scene.Add(new Predator(pos, 2, rand.Next(this.MinSpeed, this.MinSpeed + 1), rand.Next(this.MaxSpeed - 1, this.MaxSpeed + 1)));
            }

            base.Reproduce();
        }

        public void Eat(ILifeForm target)
        {
            Hunger += 100;
            Grow(target.Size);
            target.Die();
        }

        public override void AITick()
        {
            ChaseTimer--;
            if (ChaseTimer <= 0)
            {
                Target = new Point(Rand.Int(Global.Width), Rand.Int(Global.Height));
                Speed = Rand.Int(MinSpeed, MaxSpeed);
            }
            var tmp = Global.Objects.ToList();
            if (Hunger == 0) 
                Die();
            foreach (Cell target in tmp)
            {
                if (target is Herbivore)
                {
                    double Distance = (Math.Sqrt(Math.Pow(X - target.X, 2) + Math.Pow(Y - target.Y, 2)));
                    if (Distance <= 8)
                    {
                        Eat(target);
                        Speed = Rand.Int(MinSpeed, MaxSpeed);
                    }
                    else if (Distance <= Global.TargetingRadius && Target is Point)
                    {
                        ChaseTimer = 2000;
                        Target = target;
                        (target as Herbivore).Runaway();
                        if ((target as Herbivore).MaxSpeed >= MaxSpeed)
                            ChaseTimer -= 1000;
                        Speed = MaxSpeed;
                    }
                    Move();
                    return;
                }
            }
            base.AITick();
        }

        public override void Update()
        {
            AITick();
            Hunger--;
            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;
            if ((int)currentTarget.X == (int)X && (int)currentTarget.Y == (int)Y)
                    {
                        Target = new Point(Rand.Int(Global.Width), Rand.Int(Global.Height));
                        Speed = Rand.Int(MinSpeed, MaxSpeed);
                    }
            base.Update();
        }
    }
}
