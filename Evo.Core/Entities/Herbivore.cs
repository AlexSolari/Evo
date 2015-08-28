using Evo.Core.Debugging;
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
        public int GrowTimer { get; set; }

        public int ChilloutTimer { get; set; }

        public Herbivore(Point position, int size = 2, double minspeed = 1, double maxspeed = 1)
            : base(position, size, minspeed, maxspeed, Color.Green)
        {
            GrowLimit = 5;
            ResetGrowTimer();
        }

        public void ResetGrowTimer()
        {
            GrowTimer = Rand.Int(30, 40);
        }

        public override void Grow(int value)
        {
            ResetGrowTimer();
            base.Grow(value);
        }

        public override void Reproduce()
        {
            var herbivoresCount = Global.Objects.Where(x => x is Herbivore).Count();
            var maxCount = (herbivoresCount > 300) ? 1 : 3;
            for (int i = Rand.Int(1, maxCount); i >= 0; i--)
            {
                var pos = new Point((int)X + Rand.Int(-10, 10), (int)Y + Rand.Int(-10, 10));
                var entity = new Herbivore(new Point(), 2, Global.GetRandomMinSpeed(typeof(Herbivore)), Global.GetRandomMaxSpeed(typeof(Herbivore)));
                Scene.Add(entity);
                entity.SetPosition(pos.X, pos.Y);
            }
            base.Reproduce();
        }

        public override void AITick()
        {
            var self = this;
            var nearestPredator = Global.Objects
                .Where(x => x is Predator && (Math.Sqrt(Math.Pow(self.X - x.X, 2) + Math.Pow(self.Y - x.Y, 2))) < Global.SystemConfig.RanawayRadius)
                .OrderByDescending(x => (Math.Sqrt(Math.Pow(self.X - x.X, 2) + Math.Pow(self.Y - x.Y, 2))))
                .FirstOrDefault();
            if (nearestPredator != null)
                Runaway(nearestPredator);
            base.AITick();
        }

        public override void Update()
        {
            ChilloutTimer--;
            if (ChilloutTimer <= 0)
            {
                Chill();
            }
            if (AITickCounter == 0)
            {
                AITick();
                AITickCounter = Global.AITickDelay;
            }
            if (GrowTimer == 0)
            {
                Grow(1);
            }
            else
                GrowTimer--;

            base.Update();
        }

        public void Runaway(Cell from)
        {
            this.Target = new Point()
            {
                X = Convert.ToInt32(X + (X - from.X)*1.33),
                Y = Convert.ToInt32(Y + (Y - from.Y)*1.33)
            };
            Speed = MaxSpeed;
            ChilloutTimer = (ChilloutTimer <= 0) ? 800 : ChilloutTimer;
        }

        public void Chill()
        {
            Speed = MinSpeed;
        }
    }
}
