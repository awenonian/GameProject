using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Wall : Object
    {
        public Wall(Mesh mesh, Rectangle size, Manager manager) : base(mesh, new Vector2(size.X, size.Y), manager)
        {
            mesh.scale((float)size.Width / mesh.sprite.Width, (float)size.Height / mesh.sprite.Height);
        }
    }
}
