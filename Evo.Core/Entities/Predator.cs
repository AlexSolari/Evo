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
                rand = new Random(Rand.Int());
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
            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;
            var DistanceToTarget = (Math.Sqrt(Math.Pow(X - currentTarget.X, 2) + Math.Pow(Y - currentTarget.Y, 2)));

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

            if (Hunger == 0)
                Die();    
            else if (Hunger < 300)
            {
                foreach (Cell target in tmp)
                {
                    if (target is Herbivore)
                    {
                        var convertedTarget = target as Herbivore;
                        double Distance = (Math.Sqrt(Math.Pow(X - convertedTarget.X, 2) + Math.Pow(Y - convertedTarget.Y, 2)));
                        if (Distance <= Global.TargetingRadius && Distance < DistanceToTarget)
                        {
                            if (Target is Herbivore)
                                (Target as Herbivore).Chill();
                            ChaseTimer = 1500;
                            Target = target;

                            convertedTarget.Runaway(this);
                            if (convertedTarget.MaxSpeed >= MaxSpeed)
                                ChaseTimer -= 750;
                            Speed = MaxSpeed;
                            break;
                        }


                    }
                }
                Move();

                ChaseTimer--;
                if (ChaseTimer <= 0)
                {
                    Target = new Point(Rand.Int(Global.Width), Rand.Int(Global.Height));
                    Speed = Rand.Int(MinSpeed, MaxSpeed);
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
            if (Rand.Float() > 0.9)
                GameScene.Instance.Add(new Particle(X, Y, "2.png", 2, 2)
                {
                    LifeSpan = 30,
                    FinalAngle = Rand.Int(360),
                    FinalAlpha = 0,
                    FinalX = X + Rand.Int(-20, 20) * Rand.Float(),
                    FinalY = Y + Rand.Int(-20, 20) * Rand.Float(),
                    FinalScaleX = 0.5f,
                    LockScaleRatio = true
                });
        }
    }
}
