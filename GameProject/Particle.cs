using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Particle
    {
        private Vector2 position;
        private Vector2 speed;
        private Vector2 acceleration;
        private Texture2D sprite;

        private double lifeTime;

        public Particle(Vector2 position, Vector2 speed, Vector2 acceleration, Texture2D sprite, double lifeTime)
        {
            this.position = position;
            this.speed = speed;
            this.acceleration = acceleration;
            this.sprite = sprite;
            this.lifeTime = lifeTime;
        }

        /// <summary>
        /// Updates the position of the particle
        /// </summary>
        /// <returns>
        /// True if the particle needs to evaporate
        /// </returns>
        public bool update(GameTime gameTime)
        {
            speed += acceleration;
            position += speed;

            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return lifeTime <= 0;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, sprite.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
