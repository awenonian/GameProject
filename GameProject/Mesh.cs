﻿using Microsoft.Xna.Framework;
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

        public Mesh(Texture2D sprite, bool simpleCollisions)
        {

            this.sprite = sprite;
            Console.WriteLine(sprite.Width + ", " + sprite.Height);
            mesh = new List<Rectangle>();
            if (simpleCollisions)
            {
                mesh.Add(sprite.Bounds);
            }
            else
            {
                mesh = calculateMesh(sprite, 5);
            }
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
                    if (spriteData[j*sprite.Width + i].A > 127)
                    {
                        filledPixels[i, j] = true;
                    }
                    else
                    {
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
                            // Update bounds
                            canGrow = true;
                            yBound++;
                            rect.Height++;
                            
                        }
                    }

                    // Count the new pixels
                    for (int k = rect.X; k < rect.X + rect.Width; k++)
                    {
                        for (int l = rect.Y; l < rect.Y + rect.Height; l++)
                        {
                            if (!counted[k, l])
                            {
                                counted[k, l] = true;
                                newPixels++;
                            }

                        }
                    }
                    // If the rectangle matters
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
    }
}
