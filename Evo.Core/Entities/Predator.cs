//#define SHOW_HUNGER

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
    public class Predator : Cell, IPredator
    {
#if SHOW_HUNGER
        Graphic hungerText;
#endif
        public bool TargetCaptured { get; set; }
        public int Hunger { get; set; }
        public Predator(Point position, int size = 4, double minspeed = 1, double maxspeed = 1)
            : base(position, size, minspeed, maxspeed, Color.Red)
        {
#if SHOW_HUNGER
            hungerText = new Text(Hunger.ToString(), 12);
            hungerText.X = X;
            hungerText.Y = Y - 10;
#endif
            TargetCaptured = false;
            Hunger = 300;
            GrowLimit = 5;
            Global.Predators.Add(this);
        }
        public override void Reproduce()
        {
            Hunger = 500;
            base.Reproduce();
        }

        public void Eat(ILifeForm target)
        {
            Hunger += 20 * target.Size;
            Grow((int)Math.Ceiling(1.1 * target.Size / Size));
            target.Die(DyingReason.Eaten);
        }

        public override void AITick()
        {
            if (Target is Point)
                TargetCaptured = false;

            if (Hunger <= 0)
            { 
                Die(DyingReason.Hunger);
                return;
            }   
            if (Hunger < 500)
            {
                var nearestHerbivore = Global.Herbivores
                                .Where(cell => Global.DistanceSquared(this, cell) < Global.SystemConfig.TargetingRadius * Global.SystemConfig.TargetingRadius && Size >= cell.Size)
                                .OrderBy(cell => Global.DistanceSquared(this, cell))
                                .FirstOrDefault() as Herbivore;

                if (!TargetCaptured && nearestHerbivore != null)
                {
                    LockTarget(nearestHerbivore);
                }
                else if (TargetCaptured)
                {
                    if (nearestHerbivore != null && Global.Distance(this, nearestHerbivore) < Global.Distance(this, Target as Herbivore) * 1.5)
                        LockTarget(nearestHerbivore);

                    if ((Target as Herbivore).Size > Size)
                        UnlockTarget();

                    if (Hunger < 150 && Speed < MaxSpeed)
                        Speed = MaxSpeed;

                    if (Global.Distance(this, Target as Herbivore) < Global.ChargeDistance && Speed < MaxSpeed + Global.ChargeSpeedDelta)
                        Speed = MaxSpeed + Global.ChargeSpeedDelta;
                }
            }
            base.AITick();
        }


        public override void Die(DyingReason reason)
        {
#if SHOW_HUNGER
            Scene.RemoveGraphic(hungerText);
#endif
            Global.Predators.Remove(this);
            base.Die(reason);
        }

        public override void Update()
        {

#if SHOW_HUNGER
            Scene.RemoveGraphic(hungerText);
            hungerText = new Text(Hunger.ToString(), 12);
            Scene.AddGraphic(hungerText);
            hungerText.X = X - 8;
            hungerText.Y = Y - 15 - Size;
#endif
            if (AITickCounter == 0)
            {
                AITick();
                AITickCounter = Global.AITickDelay;
                
            }
            Hunger--;
            
            

            var nearestHerbivore = Global.Herbivores
                .Where(cell => Global.DistanceSquared(cell, this) < Size*Size && cell.Size <= Size)
                .OrderBy(cell => Global.DistanceSquared(cell, this))
                .FirstOrDefault() as Herbivore;
            if (nearestHerbivore != null && Target == nearestHerbivore)
            {
                Eat(nearestHerbivore);
                UnlockTarget();
            }

            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;

            if (Target == null || ((int)currentTarget.X == (int)X && (int)currentTarget.Y == (int)Y))
                UnlockTarget();
            base.Update();

        }

        public void UnlockTarget()
        {
            Target = Global.CreateRandomPoint();
            TargetCaptured = false;
            Speed = MinSpeed;
        }

        public override void CreateChilds()
        {
            var countOfChilds = 1;
            for (int i = 0; i < countOfChilds; i++)
            {
                var pos = new Point((int)X + Rand.Int(-10, 10), (int)Y + Rand.Int(-10, 10));
                Scene.Add(new Predator(pos, 4, Global.GetRandomMinSpeed(typeof(Predator)), Global.GetRandomMaxSpeed(typeof(Predator)) + 1));
            }
        }

        public void LockTarget(IHerbivore nearestHerbivore)
        {
            Target = nearestHerbivore;
            nearestHerbivore.Runaway(this);
            Speed = (MaxSpeed + MinSpeed) / 2;
            TargetCaptured = true;
        }
    }
}
