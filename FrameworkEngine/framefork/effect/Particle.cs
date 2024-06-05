using Bubla;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFramework.framefork.effect
{
    public class Particle
    {
        private Sprite sprite;
        private Color color;

        private Vector2f vector;

        public Particle() { }

        public Particle(string pathTexture, Vector2f position, float size)
        {
            color = new Color(255, 255, 255, 0);
            sprite = new Sprite();
            sprite.Color = color;
            sprite.Texture = new Texture("assets\\" + pathTexture);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            Vector2f scaleSprite = new Vector2f(size / sprite.Texture.Size.X, size / sprite.Texture.Size.Y);
            sprite.Scale = scaleSprite;
            position = new Vector2f(position.X + sprite.Texture.Size.X * sprite.Scale.X / 2f, position.Y + sprite.Texture.Size.Y * sprite.Scale.Y / 2f);
            sprite.Position = position;
        }

        public byte Alpha
        {
            get { return color.A; }
            set { 
                color.A  = value;
                sprite.Color = color;
            }
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public void UpdateVector()
        {
            vector.X = (float)(Game.Random.NextDouble() * 3) - 1;
            vector.Y = (float)(Game.Random.NextDouble() * 3) - 1;
        }

        public void Move()
        {
            sprite.Position = new Vector2f(sprite.Position.X + vector.X * Game.SDelta() * 100, sprite.Position.Y + vector.Y * Game.SDelta() * 100);
        }

        public Vector2f Position
        {
            get { return sprite.Position; }
            set
            { sprite.Position = value; }
        }

        public void Dispose()
        {
            sprite.Dispose();
        }
    }
}
