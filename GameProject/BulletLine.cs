using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class BulletLine
    {
        private Vector2 origin;
        private float rotation;
        private Manager manager;

        private Texture2D line;

        private Vector2 direction;
        float length;

        public BulletLine(Vector2 origin, float rotation, Manager manager)
        {
            this.origin = origin;
            this.rotation = rotation;
            this.manager = manager;

            direction = new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation));
            // Calculate length as distance until collision with wall
            length = findLength(manager);
        }

        private float findLength(Manager m)
        {
            List<Rectangle> walls = m.getWalls();
            foreach (Rectangle r in walls)
            {
                if (isColliding(r))
                {
                    if (direction.X < 0)
                    {
                        return (origin.X - (r.X + r.Width)) / direction.X;
                    }
                    else
                    {
                        return (r.X - origin.X) / direction.X;
                    }
                }
            }
            // Just a default value that should be off the screen, if no walls collide
            return 2048;
        }

        public Vector2 getHitPosition()
        {
            return origin + length * direction;
        }

        public bool isColliding(Rectangle rect)
        {
            // If you're behind the bullet
            if (direction.X < 0)
            {
                if (rect.X > origin.X)
                {
                    return false;
                }
            }
            else
            {
                if (rect.X + rect.Width < origin.X)
                {
                    return false;
                }
            }

            // If the top of the rectangle is below the line
            if (rect.Y < origin.Y + (origin.X - rect.X) * direction.Y && rect.Y < origin.Y + (origin.X - (rect.X + rect.Width)) * direction.Y)
            {
                return false;
            }
            // If the bottom of the rectangle is above the line
            if (rect.Y + rect.Height > origin.Y + (origin.X - rect.X) * direction.Y && rect.Y + rect.Height > origin.Y + (origin.X - rect.Width) * direction.Y)
            {
                return false;
            }

            // Otherwise, we're colliding
            return true;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(line, origin, new Rectangle(0, 0, (int) length + 1, line.Height), Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

    }
}
