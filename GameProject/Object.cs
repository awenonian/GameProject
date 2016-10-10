using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    abstract class Object
    {
        private Texture2D sprite;
        private Vector2 origin;
        private Vector2 position;

        private Manager manager;

        public Object(Texture2D sprite, Vector2 position, Manager manager)
        {
            this.sprite = sprite;
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);

            this.position = position - origin;
            Facing = 0;
            Speed = Vector2.Zero;
            Radius = 0;

            this.manager = manager;
        }

        /// <summary>
        /// Should be called when the state of the game updates. Independent from processInput.
        /// </summary>
        public virtual void update(GameTime gameTime, int width, int height)
        {
            move(gameTime, width, height);
        }

        private void move(GameTime gameTime, int width, int height)
        {
            Position += Vector2.Multiply(Speed, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Call during draw step. Draws the character.
        /// </summary>
        /// <param name="sb">
        /// The spritebatch for drawing
        /// </param>
        public virtual void draw(SpriteBatch sb)
        {
            sb.Draw(sprite,
            destinationRectangle: new Rectangle((int)(origin.X + position.X), (int)(origin.Y + position.Y), sprite.Width, sprite.Height),
            origin: origin,
            rotation: (float)Facing);
        }

        // Variables

        public Vector2 Position {
            get { return position + origin; }
            protected set { position = value - origin; }
        }

        public double Radius {
            get;
            protected set;
        }

        public Vector2 Speed
        {
            get;
            protected set;
        }

        public double Facing
        {
            get;
            protected set;
        }
    }
}
