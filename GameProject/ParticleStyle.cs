using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class ParticleStyle
    {
        public Vector2 Speed { get; private set; }
        public Vector2 Acceleration { get; private set; }
        public Texture2D Sprite { get; private set; }

        public ParticleStyle(Vector2 speed, Vector2 acceleration, Texture2D sprite)
        {
            Speed = speed;
            Acceleration = acceleration;
            Sprite = sprite;
        }
    }
}
