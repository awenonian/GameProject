﻿using Microsoft.Xna.Framework;
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
        public Mesh mesh { get; }
        private Vector2 position;

        public Manager manager;

        public Vector2 Position { get; protected set; }

        public double Radius { get; protected set; }
        public Vector2 Speed { get; protected set; }
        public double Facing { get; protected set; }

        public Object(Mesh mesh, Vector2 position, Manager manager)
        {
            this.mesh = mesh;

            Position = position;
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
            sb.Draw(mesh.sprite, Position, new Rectangle(0, 0, mesh.Width, mesh.Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public bool collision(Object other)
        {
            return mesh.collision(Position, other.Position, other.mesh);
        }
    }
}
