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

        public Plan(Vector2 origin, int count, float startAngle, float endAngle, bool regularSpread, Manager manager)
        {
            Random rand = new Random();
            shots = new BulletLine[count];
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

    }
}
