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

        public Mesh(Texture2D sprite, bool simpleCollisions)
        {
            this.sprite = sprite;
            mesh = new List<Rectangle>();
            if (simpleCollisions)
            {
                mesh.Add(sprite.Bounds);
            }
            else
            {
                mesh = calculateMesh(sprite);
            }
        }

        /// <summary>
        /// Provided a sprite, will calculate a series of boxes that contain the non transparent spaces on the sprite. 
        /// Small boxes will be removed, and concavity will be ignored.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to calculate the mesh of.
        /// </param>
        /// <returns>
        /// A List with the series of rectangles representing the sprite.
        /// </returns>
        private List<Rectangle> calculateMesh(Texture2D sprite)
        {
            List<Rectangle> mesh = new List<Rectangle>();
            Color[] spriteData = new Color[sprite.Width * sprite.Height];
            sprite.GetData(0, null, spriteData, 0, sprite.Width*sprite.Height);
            return mesh;
        }

        public bool collision(Vector2 pos, Vector2 otherPos, Mesh other)
        {
            return false;
        }
    }
}
