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

        public int Width { get { return sprite.Width; } }
        public int Height { get { return sprite.Height; } }

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
        /// Small boxes will be removed, in interest of performance
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

            // Set up: Mark whether pixels are filled or not, and mark all pixels as uncounted
            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    if (spriteData[i*sprite.Height + j].A > 127)
                    {
                        filled[i * sprite.Height + j] = Color.Black; //This is for testing
                        filledPixels[i, j] = true;
                    }
                    else
                    {
                        filled[i * sprite.Height + j] = new Color(0f, 0f, 0f, 0f); //This is for testing
                        filledPixels[i, j] = false;
                    }
                    counted[i, j] = false;
                }
            }


            for (int i = 0; i < sprite.Width; i++)
            {
                for (int j = 0; j < sprite.Height; j++)
                {
                    // Skip already counted, or empty pixels.
                    if (counted[i, j] || !filledPixels[i, j])
                    {
                        counted[i, j] = true;
                        continue;
                    }

                    // Start a new rectangle, at the current position.
                    Rectangle rect = new Rectangle(i, j, 1, 1);
                    // These represent the column and row just outside the rectangle
                    int xBound = i + 2;
                    int yBound = j + 2;

                    // How many new pixels we've added to the mesh
                    int newPixels = 0;

                    // Whether or not we can grow the rectangle
                    bool canGrow = true;
                    while (canGrow)
                    {
                        // Assume we can't grow the rectangle
                        canGrow = false;

                        // Assume we can grow to the right
                        bool growRight = true;
                        // Along the height of the rectangle
                        for (int k = 0; k < rect.Height; k++)
                        {
                            // Check if the sprite boundary allows the rectangle to grow
                            if (j + k >= sprite.Height || xBound >= sprite.Width)
                            {
                                growRight = false;
                                break;
                            }
                            // If we find an empty pixel, we can't grow
                            if (!filledPixels[xBound, j + k])
                            {
                                growRight = false;
                                break;
                            }
                        }
                        // If growth would be successful, then grow, and label that we can grow again.
                        if (growRight)
                        {
                            // Count the new pixels
                            for (int k = 0; k < rect.Height; k++)
                            {
                                if (!counted[xBound, j + k])
                                {
                                    counted[xBound, j + k] = true;
                                    newPixels++;
                                }
                            }
                            // Update bounds
                            canGrow = true;
                            xBound++;
                            rect.Width++;
                            
                        }
                        // Similar process, just along the width of the rectangle (at the bottom)
                        bool growDown = true;
                        for (int k = 0; k < rect.Width; k++)
                        {
                            if (i+k >= sprite.Width || yBound >= sprite.Height)
                            {
                                growDown = false;
                                break;
                            }
                            if (!filledPixels[i + k, yBound])
                            {
                                growDown = false;
                                break;
                            }
                        }
                        if (growDown)
                        {
                            // Count all the new pixels that we added
                            for (int k = 0; k < rect.Width; k++)
                            {
                                if (!counted[i + k, yBound])
                                {
                                    counted[i + k, yBound] = true;
                                    newPixels++;
                                }
                            }
                            // Update bounds
                            canGrow = true;
                            yBound++;
                            rect.Height++;
                            
                        }
                    }
                    // Only actually add the rectangle if it contains at least levelOfDetail new pixels
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

        public void draw(SpriteBatch sb, Vector2 position, Vector2 origin)
        {
            sb.Draw(texture: filledTex,
                destinationRectangle: new Rectangle(0, 0, sprite.Width * 8, sprite.Height * 4));
            sb.Draw(texture: boxes,
                destinationRectangle: new Rectangle(0, 0, sprite.Width * 8, sprite.Height * 4));

            /*
            sb.Draw(texture: boxes,
                destinationRectangle: new Rectangle(500, 0, sprite.Width * 8, sprite.Height * 4));
            sb.Draw(texture: filledTex,
                destinationRectangle: new Rectangle(500, 0, sprite.Width*8, sprite.Height*4));
                */

            sb.Draw(texture: boxes,
            destinationRectangle: new Rectangle((int)(origin.X + position.X), (int)(origin.Y + position.Y), sprite.Width, sprite.Height),
            origin: origin);
        }
    }
}
