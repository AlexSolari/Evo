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
        public Graphic Sprite { get; set; }
        public Color SpriteColor { get; set; }
        public int Age { get; set; }
        public int Size { get; set; }
        public Vector2 Direction { get; set; }
        public int MaxSpeed { get; set; }
        public int MinSpeed { get; set; }
        public int Speed { get; set; }
        public object Target { get; set; }
        public Cell (Point position, int size = 2, int minSpeed = 1, int maxSpeed = 2, Color spriteColor = null)
        {
            var rnd = new Random();

            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
            if (MaxSpeed < MinSpeed) MaxSpeed = MinSpeed;
            Size = size;

            Speed = Rand.Int(MinSpeed, MaxSpeed);
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
        
        public void Die()
        {
            Global.Objects.Remove(this);
            RemoveSelf();
        }
        
        public virtual void Reproduce()
        {
            Die();
        }

        public void Move()
        {
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
            SetGraphic(Image.CreateCircle(Size/2, SpriteColor));
            Graphic.CenterOrigin();
            if (Size > Global.GrowLimit)
                Reproduce();
        }

        public virtual void AITick()
        {

            var rand = new Random();
            if (X < 0 || Y < 0 || X > Global.Width || Y > Global.Height)
                Target = new Point(rand.Next(Global.Width), rand.Next(Global.Height));
            dynamic currentTarget;
            if (Target is Point)
                currentTarget = (Target as Point?).Value;
            else
                currentTarget = Target as Cell;

            var tmp = new Vector2(currentTarget.X - X, currentTarget.Y - Y);
            Global.ReduceVector(ref tmp, (float)Speed);
            Direction = tmp;
            if (Target is Point)
            {
                var targ = Target as Point?;
                if (Global.IsNear(X, targ.Value.X) && Global.IsNear(Y, targ.Value.Y))
                {
                    Target = new Point(rand.Next(Global.Width), rand.Next(Global.Height));
                    return;
                }
                else Move();
            }
        }
    }
}
