using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    class Enemy : Object
    {
        private Plan shot;

        private double shotTimer;

        private Texture2D bulletLine;

        private Random random;

        public Enemy(Mesh mesh, Vector2 position, Texture2D bulletLine, Manager manager) : base(mesh, position, manager)
        {
            shot = null;
            this.bulletLine = bulletLine;
            random = new Random();
            shotTimer = 1 + (.5 * random.NextDouble());
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            shotTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (shotTimer <= 0)
            {
                double playerAngle = Math.Atan2(manager.Player.Position.Y - Position.X, manager.Player.Position.X - Position.X);
                Console.WriteLine(playerAngle);
                shot = new Plan(Position, 3, (float) (playerAngle - Math.PI / 8), (float) (playerAngle + Math.PI / 8), false, 1, bulletLine, manager);
                shotTimer = 1.5 + (.5 * random.NextDouble());
            }

            if (shot != null)
            {
                if (shot.update(gameTime))
                {
                    shot = null;
                }
            }
            base.update(gameTime, width, height);
        }

        public override void draw(SpriteBatch sb)
        {
            if (shot != null)
            {
                shot.draw(sb);
            }
            base.draw(sb);
        }
    }
}
