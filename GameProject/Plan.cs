using Microsoft.Xna.Framework;
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

        public Plan(Vector2 origin, int count, float startAngle, float endAngle, bool regularSpread, double timeToShot, Manager manager)
        {
            this.timeToShot = timeToShot;
            this.manager = manager;

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

            if (timeToShot <= 0)
            {
                fire();
            }
        }

        public bool isSafe(Player player)
        {
            return false;
        }

        public void fire()
        {

        }

    }
}
