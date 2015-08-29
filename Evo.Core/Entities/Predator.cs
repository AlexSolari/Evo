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
    class Predator : Cell, IPredator
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
            GrowLimit = 6;
        }
        public override void Reproduce()
        {
            var countOfChilds = Rand.Int(3);
            for (int i = 0; i < countOfChilds; i++)
            {
                var pos = new Point((int)X + Rand.Int(-10, 10), (int)Y + Rand.Int(-10, 10));
                Scene.Add(new Predator(pos, Size/countOfChilds, Global.GetRandomMinSpeed(typeof(Predator)), Global.GetRandomMaxSpeed(typeof(Predator)) + 1));
            }
            Hunger = 500;
            base.Reproduce();
        }

        public void Eat(ILifeForm target)
        {
            Hunger += 20 * target.Size;
            Grow((int)Math.Ceiling(1.5 * target.Size / Size));
            target.Die();
        }

        public override void AITick()
        {
            if (Hunger <= 0)
            { 
                Die();
                return;
            }

            if (Hunger < 500)
            {
                var nearestHerbivore = Global.Objects
                                .Where(cell => cell is Herbivore && Global.Distance(this, cell) < Global.SystemConfig.TargetingRadius && Size >= cell.Size)
                                .OrderBy(cell => Global.Distance(this, cell))
                                .FirstOrDefault() as Herbivore;

                if (Target is Point && nearestHerbivore != null)
                {
                    Target = nearestHerbivore;
                    nearestHerbivore.Runaway(this);
                    Speed = (MaxSpeed + MinSpeed) / 2;
                    TargetCaptured = true;
                }
                else if (Target is Herbivore)
                {
                    if ((Target as Herbivore).Size > Size)
                        UnlockTarget();

                    if (Hunger < 150 && Speed != MaxSpeed)
                        Speed = MaxSpeed;

                    if (TargetCaptured && Global.Distance(this, Target) < Global.ChargeDistance && Speed < MaxSpeed + Global.ChargeSpeedDelta)
                        Speed = MaxSpeed + Global.ChargeSpeedDelta;
                }
            }
            base.AITick();
        }

#if SHOW_HUNGER
        public override void Die(bool allowReproduce = false)
        {
            Scene.RemoveGraphic(hungerText);
            base.Die(allowReproduce);
        }
#endif
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

            var nearestCell = Global.Objects
                .Where(cell => cell is Herbivore && Global.Distance(cell, this) < Size*2 && cell.Size <= Size)
                .OrderBy(cell => Global.Distance(cell, this))
                .FirstOrDefault() as Herbivore;
            if (nearestCell != null && Target == nearestCell)
            {
                Eat(nearestCell);
                UnlockTarget();
            }

            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;

            if (currentTarget == null || ((int)currentTarget.X == (int)X && (int)currentTarget.Y == (int)Y))
                UnlockTarget();
            base.Update();
        }

        public void UnlockTarget()
        {
            Target = Global.CreateRandomPoint();
            TargetCaptured = false;
            Speed = MinSpeed;
        }
    }
}
