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
            GrowTimer = 75;
            base.Grow(value);
        }

        public override void Reproduce()
        {
            var rand = new Random();
            var maxCount = (Global.Objects.Count > 300) ? 2 : 5;
            for (int i = rand.Next(2, maxCount); i >= 0; i--)
            {
                var pos = new Point((int)X + rand.Next(-50, 50), (int)Y + rand.Next(-50, 50));
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
                Grow(1);
            else
                GrowTimer--;

            base.Update();
        }

        public void Runaway()
        {
            Speed = MaxSpeed;
        }
    }
}
