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
        private double moveSpeed;
        private double turnSpeed;

        private double friction;

        private bool isGrounded;

        private Manager manager;

        public Player(Texture2D sprite, Vector2 position, Manager manager) : base(sprite, position, manager)
        {
            moveSpeed = 150f;
            turnSpeed = 5f;

            friction = .0001;

            isGrounded = false;

            this.manager = manager;
            Radius = 16;
            
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);
            if (isGrounded)
            {
                Speed = new Vector2(Speed.X, 0);
                //Speed *= (float)Math.Pow(friction, gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (!isGrounded)
            {
                Speed += (float) (gameTime.ElapsedGameTime.TotalSeconds) * new Vector2(0, 1000f);
            }
            if (Position.Y > 300)
            {
                isGrounded = true;
            }
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

            if (state.IsKeyDown(Keys.Space) && isGrounded)
            {
                Speed += new Vector2(0, -150f);
                isGrounded = false;
            }
            if (state.IsKeyDown(Keys.A))
            {
                Speed = new Vector2((float) -moveSpeed, Speed.Y);
            }
            else
            {
                // This should stop the character if no sideways key is pressed
                Speed = new Vector2(0, Speed.Y);
            }
            if (state.IsKeyDown(Keys.D))
            {
                Speed = new Vector2((float)moveSpeed, Speed.Y);
            }
        }
    }
}
