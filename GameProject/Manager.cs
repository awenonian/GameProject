using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Manager
    {
        private Texture2D background;
        private Texture2D gameOverScreen;
        //player sprites
        private Texture2D playerSprite;

        private List<Object> objects;
        private Object player;

        /// <summary>
        /// Width of the game screen.
        /// </summary>
        private int width;
        /// <summary>
        /// Height of the game screen.
        /// </summary>
        private int height;

        private bool gameOver;
        private int playerBufferSpace;

        private Random r;

        public Manager()
        {
            objects = new List<Object>();
            height = 0;
            width = 0;
            player = null;
            r = new Random();
            gameOver = false;
            playerBufferSpace = 100;
        }

        public void initialize(int width, int height)
        {
            this.width = width;
            this.height = height;

        }

        public void loadContent(ContentManager content)
        {
            //<variable> = content.Load<Texture2D>(<filename - extension>);
            //e.g. gameOverScreen = content.Load<Texture2D>("GameOver");
        }

        public void update(GameTime gameTime, KeyboardState state, KeyboardState previousState)
        {

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                objects[i].update(gameTime, width, height);
            }

            //player.processInput(state, previousState, gameTime);
            player.update(gameTime, width, height);

        }

        public void add(Object o)
        {
            objects.Add(o);
        }

        public void draw(SpriteBatch sb)
        {
            if (!gameOver)
            {
                sb.Draw(background, destinationRectangle: new Rectangle(0, 0, width, height));
                foreach (Object o in objects)
                {
                    o.draw(sb);
                }

                player.draw(sb);
            }
            else
            {
                sb.Draw(gameOverScreen, destinationRectangle: new Rectangle(0, 0, width, height));
            }
        }
    }
}
