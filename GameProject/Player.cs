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
        private double floatTime;

        private Manager manager;

        public Player(Mesh mesh, Vector2 position, Manager manager) : base(mesh, position, manager)
        {
            //All of these numbers need tweaking.
            moveSpeed = 150f;
            jumpSpeed = 1000f;
            dashLength = 100f;
            floatTime = .25;
            gravity = new Vector2(0, 3000f);

            isGrounded = false;

            isFloating = false;
            floatTimer = 0;

            this.manager = manager;
            Radius = 16;
            
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);
            /*
            if (isGrounded)
            {
                Speed = new Vector2(Speed.X, 0);
            }
            */

            Wall w;
            w = manager.wallCollision(this);
            if (w != null)
            {
                onCollision(w);
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

        public void onCollision(Object o)
        {
            isGrounded = true;
            isFloating = false;

            if (!Speed.Equals(Vector2.Zero))
            {
                Vector2 currentSpeed = Speed;
                currentSpeed.Normalize();

                while (collision(o))
                {
                    Position -= currentSpeed;
                }
            }
            Speed = new Vector2(Speed.X, 0);
        }

        public void processInput(GamePadState state, GamePadState prevState, GameTime gameTime)
        {
            if (state.ThumbSticks.Left.Length() > .25)
            {
                Speed = new Vector2(moveSpeed * state.ThumbSticks.Left.X, Speed.Y);
            }
            else
            {
                Speed = new Vector2(0, Speed.Y);
            }
            if (isGrounded)
            {
                if (state.IsButtonDown(Buttons.A))
                {
                    Speed = new Vector2(Speed.X, -jumpSpeed);
                    isGrounded = false;
                }
            }
            else
            {
                if (state.IsButtonDown(Buttons.A) && !prevState.IsButtonDown(Buttons.A))
                {
                    bool airDash = false;
                    Vector2 dashVec = Vector2.Zero;
                    if (state.ThumbSticks.Left.Length() > .25)
                    {
                        dashVec = state.ThumbSticks.Left;
                        dashVec.Y = -dashVec.Y;
                        airDash = true;
                    }
                    if (airDash)
                    {
                        dash(dashVec);
                    }
                }
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
            if (isGrounded)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    Speed = new Vector2(Speed.X, -jumpSpeed);
                    isGrounded = false;
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
                        dash(dashVec);
                    }
                }
            }
        }

        /// <summary>
        /// Dashes the player in the given direction
        /// </summary>
        /// <param name="dashVec">
        /// A vector pointing in the direction of the dash
        /// </param>
        private void dash(Vector2 dashVec)
        {
            dashVec.Normalize();
            dashVec *= Math.Min(mesh.Width / Math.Abs(dashVec.X), mesh.Height / Math.Abs(dashVec.Y));
            float dashVecLength = dashVec.Length();
            float dashProgress = 0;
            Wall w = null;
            while (dashProgress + dashVecLength < dashLength)
            {
                Position += dashVec;
                w = manager.wallCollision(this);
                if (w != null)
                {
                    dashVec.Normalize();
                    while (w.collision(this))
                    {
                        Position -= dashVec;
                    }
                    floatTimer = floatTime;
                    isFloating = true;
                    Speed = Vector2.Zero;
                    return;
                }
                dashProgress += dashVecLength;
            }
            dashVec.Normalize();
            Position += (dashLength - dashProgress) * dashVec;
            w = manager.wallCollision(this);
            if (w != null)
            {
                dashVec.Normalize();
                while (w.collision(this))
                {
                    Position -= dashVec;
                }
            }
            floatTimer = floatTime;
            isFloating = true;
            Speed = Vector2.Zero;
        }
    }
}
