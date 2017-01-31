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
        private static double bufferTime = .2;
        private static int numShots = 0; //Temp variable for reporting
        public Color color { get; set; }

        public Plan(Vector2 origin, int count, float startAngle, float endAngle, bool regularSpread, double timeToShot, Texture2D bulletLine, Manager manager)
        {
            this.timeToShot = timeToShot;
            this.manager = manager;
            color = Color.White;

            shots = new BulletLine[count];
            Random rand = new Random();
            for (int i = 0; i < count; i ++)
            {
                if (regularSpread) {
                    shots[i] = new BulletLine(origin, startAngle + i * (endAngle - startAngle), bulletLine, manager);
                }
                else
                {
                    shots[i] = new BulletLine(origin, (float) rand.NextDouble() * (endAngle - startAngle) + startAngle, bulletLine, manager);
                }
            }
        }

        public Plan(BulletLine[] shots, double timeToShot, Texture2D bulletLine, Manager manager)
        {
            this.shots = shots;
            this.timeToShot = timeToShot;
            this.manager = manager;
        }

        /// <summary>
        /// updates the timer of the plan, fires if it's time to fire.
        /// </summary>
        /// <param name="gameTime">
        /// The game timer for getting the elapsed time since last update
        /// </param>
        /// <returns>
        /// True if it fired and needs to be deconstructed
        /// </returns>
        public bool update(GameTime gameTime)
        {
            timeToShot -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeToShot <= bufferTime && isSafe(manager.Player))
            {
                fire(true);
                return true;
            }
            else if (timeToShot <= 0)
            {
                fire(isSafe(manager.Player));
                return true;
            }
            return false;
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
            numShots++;
            Console.WriteLine(numShots + ": FIRING!");
            if (!safe)
            {
                manager.Player.hit();
            }
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
