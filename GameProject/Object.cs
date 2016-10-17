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
        private Mesh mesh;
        private Vector2 origin;
        private Vector2 position;

        private Manager manager;

        public Vector2 Position
        {
            get { return position + origin; }
            protected set { position = value - origin; }
        }

        public double Radius { get; protected set; }
        public Vector2 Speed { get; protected set; }
        public double Facing { get; protected set; }

        public Object(Mesh mesh, Vector2 position, Manager manager)
        {
            this.mesh = mesh;
            origin = new Vector2(mesh.Width / 2, mesh.Height / 2);

            this.position = position - origin;
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
        public void draw(SpriteBatch sb)
        {
            mesh.draw(sb, position, origin);
            sb.Draw(mesh.sprite,
            destinationRectangle: new Rectangle((int)(origin.X + position.X), (int)(origin.Y + position.Y), mesh.sprite.Width, mesh.sprite.Height),
            origin: origin,
            rotation: (float)Facing);
        }

        public bool collision(Object other)
        {
            return mesh.collision(Position, other.Position, other.mesh);
        }
    }
}
