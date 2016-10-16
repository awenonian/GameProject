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
        private float moveSpeed;
        private float jumpSpeed;
        private float dashLength;

        private Vector2 gravity;

        private bool isGrounded;

        private bool isFloating;
        private double floatTimer;

        private Manager manager;

        public Player(Texture2D sprite, Vector2 position, Manager manager) : base(sprite, position, manager)
        {
            //All of these numbers need tweaking.
            moveSpeed = 150f;
            jumpSpeed = 500f;
            dashLength = 100f;

            gravity = new Vector2(0, 5000f);

            isGrounded = false;

            isFloating = false;
            floatTimer = 0;

            this.manager = manager;
            Radius = 16;
            
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);
            if (isGrounded)
            {
                Speed = new Vector2(Speed.X, 0);
            }
            if (!isGrounded && !isFloating)
            {
                //Gravity
                Speed += (float)(gameTime.ElapsedGameTime.TotalSeconds) * gravity;
            }
            if (Position.Y > 300) //This is the temporary ground, later we'll put collision detection in this statement
            {
                isGrounded = true;
                isFloating = false;
            }
            floatTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (floatTimer < 0)
            {
                isFloating = false;
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
            if (isGrounded)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    Speed += new Vector2(0, -jumpSpeed);
                    isGrounded = false;
                }
                if (state.IsKeyDown(Keys.A))
                {
                    Speed = new Vector2(-moveSpeed, Speed.Y);
                }
                else
                {
                    // This should stop the character if no sideways key is pressed
                    Speed = new Vector2(0, Speed.Y);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    Speed = new Vector2(moveSpeed, Speed.Y);
                }
            }
            else
            {
                if (state.IsKeyDown(Keys.Space) && !prevState.IsKeyDown(Keys.Space))
                {
                    bool airDash = false;
                    Vector2 dashVec = new Vector2(0, 0);
                    if (state.IsKeyDown(Keys.W))
                    {
                        dashVec.Y -= 1;
                        airDash = true;
                    }
                    if (state.IsKeyDown(Keys.S))
                    {
                        dashVec.Y += 1;
                        airDash = true;
                    }
                    if (state.IsKeyDown(Keys.A))
                    {
                        dashVec.X -= 1;
                        airDash = true;
                    }
                    if (state.IsKeyDown(Keys.D))
                    {
                        dashVec.X += 1;
                        airDash = true;
                    }
                    if (airDash)
                    {
                        dashVec.Normalize();
                        Position += dashLength * dashVec;
                        floatTimer = .5;
                        isFloating = true;
                        Speed = Vector2.Zero;
                    }
                }
            }
        }
    }
}
