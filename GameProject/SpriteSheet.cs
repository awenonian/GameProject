using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class SpriteSheet
    {
        private static double animationTime = .25;

        List<List<Texture2D>> sheet;

        int state;

        double elapsedTime;

        public SpriteSheet(Texture2D spriteSheet, int width, int height, GraphicsDevice gd)
        {
            sheet = new List<List<Texture2D>>();
            state = 0;
            elapsedTime = 0;

            for (int i = 0; i < spriteSheet.Height; i += height)
            {
                sheet.Add(new List<Texture2D>());
                for (int j = 0; j < spriteSheet.Width; j += width)
                {
                    Color[] colors = new Color[width * height];
                    Texture2D sprite = new Texture2D(gd, width, height);
                    spriteSheet.GetData(0, new Rectangle(j, i, width, height), colors, 0, width * height);
                    sprite.SetData(colors);
                    // If any of the pixels is not fully transparent
                    if (colors.Any(c => c.A != 0))
                    {
                        sheet[i].Add(sprite);
                    }
                }
            }
        }

        public void setState(int i)
        {
            state = i % sheet.Count();
            elapsedTime = 0;
        }

        public void draw(SpriteBatch sb, Vector2 position, int width, int height, GameTime gt)
        {
            elapsedTime += gt.ElapsedGameTime.TotalSeconds;

            int index = (int)(elapsedTime / animationTime) % sheet[state].Count();

            sb.Draw(sheet[state][index], position, new Rectangle(0, 0, width, height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
