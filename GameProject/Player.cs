using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Player : Object
    {
        private double thrust;
        private double turnSpeed;

        private double friction;

        private Manager manager;

        public Player(Texture2D sprite, Vector2 position, Manager manager) : base(sprite, position, manager)
        {
            thrust = 300f;
            turnSpeed = 5f;

            friction = .5;

            this.manager = manager;
            Radius = 16;
            
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);

            Speed *= (float)Math.Pow(friction, gameTime.ElapsedGameTime.TotalSeconds);
            if (Speed.Length() < 1)
            {
                Speed = Vector2.Zero;
            }
        }

        /// <summary>
        /// This is during the input step. Takes input, and produces the necessary changes
        /// </summary>
        /// <param name="state">
        /// The current state of the keyboard
        /// </param>
        public void processInput(KeyboardState state, KeyboardState prevState, GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (state.IsKeyDown(Keys.W))
            {
                accelerate(thrust * elapsedTime);
            }
            if (state.IsKeyDown(Keys.A))
            {
                rotate(-turnSpeed * elapsedTime);
            }
            if (state.IsKeyDown(Keys.S))
            {
                accelerate(-thrust * elapsedTime);
            }
            if (state.IsKeyDown(Keys.D))
            {
                rotate(turnSpeed * elapsedTime);
            }
        }

        private void rotate(double rotation)
        {
            Facing += rotation;
        }

        private void accelerate(double thrust)
        {
            Speed += new Vector2((float)(Math.Cos(Facing) * thrust), (float)(Math.Sin(Facing) * thrust));
        }
    }
}
