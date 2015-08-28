﻿using Evo.Core.Debugging;
using Evo.Core.Interfaces;
using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evo.Core.Entities
{
    public abstract class Cell : Entity, ILifeForm, IReproductable, IMovable, IGrowable, IAIControllable
    {
        public int AITickCounter { get; set; }
        public Graphic Sprite { get; set; }
        public Color SpriteColor { get; set; }
        public int Age { get; set; }
        public int Size { get; set; }
        public Vector2 Direction { get; set; }
        public double MaxSpeed { get; set; }
        public double MinSpeed { get; set; }
        public double Speed { get; set; }
        public object Target { get; set; }
        public int GrowLimit { get; set; }
        public Cell(Point position, int size = 2, double minSpeed = 1, double maxSpeed = 1, Color spriteColor = null)
        {
            var rnd = new Random();

            MinSpeed = minSpeed;
            MaxSpeed = (maxSpeed > 5) ? 5 : maxSpeed;
            if (MinSpeed < 1) MinSpeed = 1;
            if (MaxSpeed < MinSpeed) MaxSpeed = MinSpeed;
            Size = size;

            Speed = MinSpeed;
            SpriteColor = spriteColor;
            Sprite = Image.CreateCircle(Size, SpriteColor);
            Direction = new Vector2();
            Target = new Point(rnd.Next(Global.Width), rnd.Next(Global.Height));

            Graphic = Sprite;
            Graphic.CenterOrigin();
            X = position.X;
            Y = position.Y;

            Global.Objects.Add(this);
        }

        public void Destroy()
        {
            RemoveSelf();
        }
        
        public void Die()
        {
            Global.Objects.Remove(this);
            Destroy();
        }

        public virtual void Reproduce()
        {
            Age += 1;
            if (Age > 3)
                Die();
        }

        public void Move()
        {
            if (X <= 0 || Y <= 0 || X >= Global.Width || Y >= Global.Height)
            {
                Target = Global.CreateRandomPoint();
                while (X <= 0) X+=5;
                while (Y <= 0) Y+=5;
                while (X >= Global.Width) X-=5;
                while (Y >= Global.Height) Y-=5;
            }

            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;
            var tmp = new Vector2(currentTarget.X - X, currentTarget.Y - Y);

            Global.ReduceVector(ref tmp, (float)Speed);

            Direction = tmp;

            this.X += Direction.X;
            this.Y += Direction.Y;
        }

        public void Rotate(float angle)
        {

            throw new NotImplementedException();
        }

        public virtual void Grow(int value)
        {
            Size += value;
            Sprite = Image.CreateCircle(Size/2, SpriteColor);
            Graphic = Sprite;
            Graphic.CenterOrigin();
            if (Size > GrowLimit * 2)
                Die();
            if (Size > GrowLimit && Global.Objects.Count < 300)
                Reproduce();
        }

        public virtual void AITick()
        {
            if (Target is Point)
            {
                var targ = (Target as Point?).Value;
                if (Global.IsNear(X, targ.X) && Global.IsNear(Y, targ.Y))
                {
                    Target = Global.CreateRandomPoint();
                }  
            }
                
            
        }

        public override void Update()
        {
            Move();
            AITickCounter--;
        }
    }
}
