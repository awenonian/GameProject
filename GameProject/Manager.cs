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
        private List<Wall> walls;
        private List<BulletLine> bulletLines;
        public Player Player { get; private set; }

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
            walls = new List<Wall>();
            bulletLines = new List<BulletLine>();
            height = 0;
            width = 0;
            Player = null;
            r = new Random();
        }

        public void initialize(int width, int height)
        {
            this.width = width;
            this.height = height;

            Player = new Player(playerMesh, new Vector2(300, 100), this);
            walls.Add(new Wall(wallMesh, new Rectangle(0, 300, 512, 32), this));
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
                Player.processInput(kState, prevKState, gameTime);
            }
            // Otherwise, assume Game Pad controls
            else
            {
                Player.processInput(gState, prevGState, gameTime);
            }
            Player.update(gameTime, width, height);
            
        }

        public void draw(SpriteBatch sb)
        {
            //sb.Draw(background, destinationRectangle: new Rectangle(0, 0, width, height));
            foreach (Object o in objects)
            {
                o.draw(sb);
            }
            foreach (Wall w in walls)
            {
                w.draw(sb);
            }
            Player.draw(sb);
        }

        public Object collision(Object obj)
        {
            foreach (Object o in objects)
            {
                if (obj.collision(o))
                {
                    return o;
                }
            }
            return null;
        }

        public Wall wallCollision(Object obj)
        {
            foreach (Wall w in walls)
            {
                if (obj.collision(w))
                {
                    return w;
                }
            }
            return null;
        }

        public List<Rectangle> getWalls()
        {
            List<Rectangle> rects = new List<Rectangle>();
            foreach (Wall w in walls)
            {
                // Walls should always be simple, so they're mesh's should only have 1 rectangle
                rects.Add(w.mesh.mesh[0]);
            }
            return rects;
        }
    }
}
