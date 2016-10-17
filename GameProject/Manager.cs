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
        //player sprites
        private Texture2D playerSprite;
        private Mesh playerMesh;

        private Texture2D wallSprite;
        private Mesh wallMesh;

        private Wall testWall;

        private List<Object> objects;
        private Player player;

        /// <summary>
        /// Width of the game screen.
        /// </summary>
        private int width;
        /// <summary>
        /// Height of the game screen.
        /// </summary>
        private int height;

        private Random r;

        public Manager()
        {
            objects = new List<Object>();
            height = 0;
            width = 0;
            player = null;
            r = new Random();
        }

        public void initialize(int width, int height)
        {
            this.width = width;
            this.height = height;

            player = new Player(playerMesh, new Vector2(300, 100), this);
            testWall = new Wall(wallMesh, new Rectangle(256, 16, 512, 32), this);
        }

        public void loadContent(ContentManager content)
        {
            //<variable> = content.Load<Texture2D>(<filename - extension>);
            playerSprite = content.Load<Texture2D>("mario-small");
            playerMesh = new Mesh(playerSprite, false);

            wallSprite = content.Load<Texture2D>("Wall");
            wallMesh = new Mesh(wallSprite, true);
        }

        public void update(GameTime gameTime, KeyboardState kState, KeyboardState prevKState, GamePadState gState, GamePadState prevGState)
        {

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                objects[i].update(gameTime, width, height);
            }
            // If someone is operating keyboard
            if (kState.GetPressedKeys().Length != 0)
            {
                player.processInput(kState, prevKState, gameTime);
            }
            // Otherwise, assume Game Pad controls
            else
            {
                player.processInput(gState, prevGState, gameTime);
            }
            player.update(gameTime, width, height);

        }

        public void add(Object o)
        {
            objects.Add(o);
        }

        public void draw(SpriteBatch sb)
        {
            //sb.Draw(background, destinationRectangle: new Rectangle(0, 0, width, height));
            foreach (Object o in objects)
            {
                o.draw(sb);
            }
            testWall.draw(sb);
            player.draw(sb);
        }
    }
}
