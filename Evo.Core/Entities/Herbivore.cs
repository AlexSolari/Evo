using Evo.Core.Interfaces;
using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Entities
{
    class Herbivore : Cell, IHerbivore
    {
        public int GrowTimer { get; private set; }

        public Herbivore(Point position ,int size = 2, int minspeed = 1, int maxspeed = 2) : base(position , size, minspeed, maxspeed, Color.Green)
        {
            GrowTimer = 75;
        }

        public override void Grow(int value)
        {
            GrowTimer = 100;
            base.Grow(value);
        }

        public override void Reproduce()
        {
            var rand = new Random();
            var maxCount = (Global.Objects.Count > 100) ? 3 : 6;
            for (int i = rand.Next(2, maxCount); i >= 0; i--)
            {
                rand = new Random(Rand.Int());
                var pos = new Point((int)X + rand.Next(-10, 10), (int)Y + rand.Next(-10, 10));
                var entity = new Herbivore(new Point(), 2, rand.Next(this.MinSpeed, this.MinSpeed + 1), rand.Next(this.MaxSpeed - 1, this.MaxSpeed + 1));
                Scene.Add(entity);
                entity.SetPosition(pos.X, pos.Y);
            }

            base.Reproduce();
        }

        public override void AITick()
        {
            base.AITick();
        }

        public override void Update()
        {
            AITick();

            if (GrowTimer == 0)
                Grow(Rand.Int(1, 3));
            else
                GrowTimer--;

            base.Update();
            if (Rand.Float() > 0.9 && Global.Objects.Count < 100)
                GameScene.Instance.Add(new Particle(X, Y, "1.png", 2, 2)
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

        public void Runaway(Cell from)
        {
            this.Target = new Point()
            {
                X = Convert.ToInt32(X + (X - from.X)*1.33),
                Y = Convert.ToInt32(Y + (Y - from.Y)*1.33)
            };
            Speed = MaxSpeed;
        }

        public void Chill()
        {
            Speed = MinSpeed;
        }
    }
}
