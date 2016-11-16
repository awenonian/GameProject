using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class ParticleManager
    {
        private List<Particle> particles;
        private Random random;

        public ParticleManager()
        {
            particles = new List<Particle>();
            random = new Random();
        }

        public void addParticles(ParticleStyle p, int count, Vector2 origin, float radius)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 displacement = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                displacement.Normalize();
                displacement *= radius * (float) random.NextDouble();
                particles.Add(new Particle(origin + displacement, p.Speed, p.Acceleration, p.Sprite, random.NextDouble()));
            }
        }

        public void update(GameTime gameTime)
        {
            for (int i = particles.Count-1; i >= 0; i--)
            {
                if (particles[i].update(gameTime))
                {
                    particles.Remove(particles[i]);
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            foreach (Particle p in particles)
            {
                p.draw(sb);
            }
        }
    }
}
