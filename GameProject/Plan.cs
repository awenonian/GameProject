using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Plan
    {
        private BulletLine[] shots;
        private Manager manager;
        private double timeToShot;
        private static double bufferTime = .5;
        public Color color { get; set; }

        public Plan(Vector2 origin, int count, float startAngle, float endAngle, bool regularSpread, double timeToShot, Manager manager)
        {
            this.timeToShot = timeToShot;
            this.manager = manager;
            color = Color.White;

            shots = new BulletLine[count];
            Random rand = new Random();
            for (int i = 0; i < count; i ++)
            {
                if (regularSpread) {
                    shots[i] = new BulletLine(origin, startAngle + i * (endAngle - startAngle), manager);
                }
                else
                {
                    shots[i] = new BulletLine(origin, (float) rand.NextDouble() * (endAngle - startAngle) + startAngle, manager);
                }
            }
        }

        public Plan(BulletLine[] shots, double timeToShot, Manager manager)
        {
            this.shots = shots;
            this.timeToShot = timeToShot;
            this.manager = manager;
        }

        public void update(GameTime gameTime)
        {
            timeToShot -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeToShot <= bufferTime && isSafe(manager.Player))
            {
                fire(true);
            }
            else if (timeToShot <= 0)
            {
                fire(isSafe(manager.Player));
            }
        }

        public bool isSafe(Player player)
        {
            foreach (BulletLine bl in shots)
            {
                if (bl.isColliding(player.Position, player.mesh))
                {
                    return true;
                }
            }
            return false;
        }

        public void fire(bool safe)
        {
            if (!safe)
            {
                manager.Player.hit();
            }
            // Deconstruct
        }

        public void draw(SpriteBatch sb)
        {
            foreach (BulletLine bl in shots)
            {
                bl.draw(sb, color);
            }
        }

    }
}
