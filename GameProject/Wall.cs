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
        public Wall(Mesh mesh, Vector2 position, Manager manager) : base(mesh, position, manager)
        {

        }
    }
}
