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
    public class Herbivore : Cell, IHerbivore
    {
        IPredator Chaser = null;
        public int GrowTimer { get; set; }

        public int ChilloutTimer { get; set; }

        public Herbivore(Point position, int size = 2, double minspeed = 1, double maxspeed = 1)
            : base(position, size, minspeed, maxspeed, Color.Green)
        {
            if (maxspeed > 3) maxspeed = 3;
            GrowLimit = 6;
            ResetGrowTimer();
            Global.Herbivores.Add(this);
        }

        public override void Die(DyingReason reason)
        {
            if (Chaser != null)
                Chaser.UnlockTarget();
            Global.Herbivores.Remove(this);
            base.Die(reason);
        }

        public void ResetGrowTimer()
        {
            GrowTimer = Rand.Int(20, 30);
        }

        public override void Grow(int value)
        {
            ResetGrowTimer();
            base.Grow(value);
        }

        public override void Reproduce()
        {
            if (Chaser != null)
                Chaser.UnlockTarget();
            base.Reproduce();
        }

        public override void AITick()
        {
            if (Chaser != null)
            {
                var nearestPredator = Global.Predators
                .Where(x => Global.DistanceSquared(x, this) < Global.SystemConfig.RanawayRadius)
                .OrderBy(x => Global.DistanceSquared(x, this))
                .FirstOrDefault();
                if (nearestPredator != null)
                    Runaway(nearestPredator as Predator, 300);
            }
            base.AITick();
        }

        public override void Update()
        {
            if (ChilloutTimer <= 0)
            {
                Chill();
            }
            else ChilloutTimer--;
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

        public void Runaway(Predator chaser, int chilloutTimer = 500)
        {
            this.Target = new Point()
            {
                X = Convert.ToInt32(X + (X - chaser.X)*1.33),
                Y = Convert.ToInt32(Y + (Y - chaser.Y)*1.33)
            };
            Speed = MaxSpeed;
            Chaser = chaser;
            ChilloutTimer = chilloutTimer;
        }

        public void Chill()
        {
            if (Chaser != null)
                Chaser = null;
            Speed = MinSpeed;
        }

        public override void CreateChilds()
        {
            var herbivoresCount = Global.Herbivores.Count();
            var countOfChilds = (herbivoresCount > Global.HerbivoresLimit) ? 1 : 7;
            for (int i = countOfChilds; i >= 0; i--)
            {
                var pos = new Point((int)X + Rand.Int(-10, 10), (int)Y + Rand.Int(-10, 10));
                var entity = new Herbivore(new Point(), 2, Global.GetRandomMinSpeed(typeof(Herbivore)), Global.GetRandomMaxSpeed(typeof(Herbivore)));
                Scene.Add(entity);
                entity.SetPosition(pos.X, pos.Y);
            }
        }
    }
}
