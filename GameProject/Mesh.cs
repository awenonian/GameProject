using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Mesh
    {
        public Texture2D sprite { get; private set; }
        public List<Rectangle> mesh { get; private set; }

        public int Width { get { return sprite.Width; } private set { } }
        public int Height { get { return sprite.Height; } private set { } }

        //For testing:
        private Texture2D boxes;

        private Color[] filled;
        private Texture2D filledTex;

        public Mesh(Texture2D sprite, bool simpleCollisions, GraphicsDevice graphicsDevice)
        {
            filled = new Color[sprite.Width * sprite.Height];

            this.sprite = sprite;
            mesh = new List<Rectangle>();
            if (simpleCollisions)
            {
                mesh.Add(sprite.Bounds);
            }
            else
            {
                mesh = calculateMesh(sprite, 5);
            }

            filledTex = new Texture2D(graphicsDevice, sprite.Width, sprite.Height);
            filledTex.SetData(filled);

            //For Testing:
            Color[] colors = { Color.AliceBlue, Color.AntiqueWhite, Color.Beige, Color.Black, Color.BlanchedAlmond, Color.BlueViolet, Color.BurlyWood, Color.Chartreuse, Color.Coral, Color.Crimson, Color.DarkBlue };
            boxes = new Texture2D(graphicsDevice, sprite.Width, sprite.Height);
            Color[] data = new Color[sprite.Width * sprite.Height];
            int index = 0;
            foreach (Rectangle r in mesh)
            {
                for (int i = r.X; i < r.X + r.Width; i++)
                {
                    for (int j = r.Y; j < r.Y + r.Height; j++)
                    {
                        data[i * sprite.Width + j] = colors[index];
                    }
                }
                index = (index + 1) % colors.Length;
            }
            boxes.SetData(data);
        }

        /// <summary>
        /// Provided a sprite, will calculate a series of boxes that contain the non transparent spaces on the sprite. 
        /// Small boxes will be removed, and concavity will be ignored.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to calculate the mesh of.
        /// </param>
        /// <param name="levelOfDetail">
        /// The amount of new pixels a rectangle must include to be included. Lower numbers are higher detail, higher numbers are faster.
        /// </param>
        /// <returns>
        /// A List with the series of rectangles representing the sprite.
        /// </returns>
        private List<Rectangle> calculateMesh(Texture2D sprite, int levelOfDetail)
        {
            List<Rectangle> mesh = new List<Rectangle>();
            Color[] spriteData = new Color[sprite.Width * sprite.Height];
            sprite.GetData(0, null, spriteData, 0, sprite.Width*sprite.Height);
            bool[,] filledPixels = new bool[sprite.Width, sprite.Height];
            bool[,] counted = new bool[sprite.Width, sprite.Height];
            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    if (spriteData[i*sprite.Height + j].A > 127)
                    {
                        filled[i * sprite.Height + j] = Color.Black;
                        filledPixels[i, j] = true;
                    } else
                    {
                        filled[i * sprite.Height + j] = Color.White;
                        filledPixels[i, j] = false;
                    }
                    counted[i, j] = false;
                }
            }
            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    if (counted[i, j] || !filledPixels[i, j])
                    {
                        counted[i, j] = true;
                        continue;
                    }

                    Rectangle rect = new Rectangle(i, j, 0, 0);
                    int xBound = i + 1;
                    int yBound = j + 1;

                    int newPixels = 0;

                    bool canGrow = true;
                    while (canGrow)
                    {
                        canGrow = false;
                        bool growRight = true;
                        for (int k = 0; k < rect.Height; k++)
                        {
                            if (j + k >= counted.GetLength(1) || xBound >= counted.GetLength(0))
                            {
                                growRight = false;
                                break;
                            }
                            if (!counted[xBound, j + k])
                            {
                                counted[xBound, j + k] = true;
                                newPixels++;
                            }
                            if (!filledPixels[xBound, j + k])
                            {
                                growRight = false;
                                break;
                            }
                        }
                        if (growRight)
                        {
                            canGrow = true;
                            xBound++;
                            rect.Width++;
                        }

                        bool growDown = true;
                        for (int k = 0; k < rect.Width; k++)
                        {
                            if (i+k >= counted.GetLength(0) || yBound >= counted.GetLength(1))
                            {
                                growDown = false;
                                break;
                            }
                            if (!counted[i + k, yBound])
                            {
                                counted[i + k, yBound] = true;
                                newPixels++;
                            }
                            if (!filledPixels[i + k, yBound])
                            {
                                growDown = false;
                                break;
                            }
                        }
                        if (growDown)
                        {
                            canGrow = true;
                            yBound++;
                            rect.Height++;
                        }
                    }
                    if (newPixels > levelOfDetail)
                    {
                        mesh.Add(rect);
                    }
                }
            }
            return mesh;
        }

        public bool collision(Vector2 pos, Vector2 otherPos, Mesh other)
        {
            // This should take care of most objects.
            if (otherPos.X > pos.X + Width)
            {
                return false;
            }
            if (otherPos.Y > pos.Y + Height)
            {
                return false;
            }
            if (otherPos.X + other.Width < pos.X)
            {
                return false;
            }
            if (otherPos.Y + other.Height < pos.Y)
            {
                return false;
            }
            // Here is more intenstive checking
            for (int i = 0; i < mesh.Count; i++)
            {
                for (int j = 0; j < other.mesh.Count; j++)
                {
                    //This is the same check as above, just rewritten
                    if (!((otherPos.X > pos.X + Width) 
                        && (otherPos.Y > pos.Y + Height) 
                        && (otherPos.X + other.Width < pos.X)
                        && (otherPos.Y + other.Height < pos.Y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture: boxes, position: Vector2.Zero);
            sb.Draw(texture: filledTex, position: new Vector2(100, 0));
        }
    }
}
