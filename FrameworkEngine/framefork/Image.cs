using Box2DX.Dynamics;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Bubla
{
    public class Image
    {
        private static Dictionary<string, Image> images = new Dictionary<string, Image>();

        private Vector2f defaultScale;
        private Sprite sprite;
        private string name;
        private string pathTexture;

        private bool active;
        private bool veryUpLayer;

        public Image() { }

        public Image(string name, string path, Vector2f position, Vector2f scale)
        {
            this.name = name;
            this.pathTexture = path;
            this.defaultScale = scale;
            this.active = true;

            sprite = new Sprite();
            sprite.Texture = new Texture("assets\\" + pathTexture);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            Vector2f scaleSprite = new Vector2f(scale.X / sprite.Texture.Size.X, scale.Y / sprite.Texture.Size.Y);
            sprite.Scale = scaleSprite;
            position = new Vector2f(position.X + sprite.Texture.Size.X * sprite.Scale.X / 2f, position.Y + sprite.Texture.Size.Y * sprite.Scale.Y / 2f);
            sprite.Position = position;

            images.Add(name, this);
        }

        public Image Get(string name)
        {
            return images[name];
        }

        public string Name
        {
            get { return name; }
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public Vector2f Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        public Vector2f Scale
        {
            get { return sprite.Scale; }
            set { sprite.Scale = value; }
        }

        public void AddPosition(float x, float y)
        {
            sprite.Position = new Vector2f(sprite.Position.X + x, sprite.Position.Y + y);
        }

        public void SetScale(float x, float y)
        {
            sprite.Scale = new Vector2f(x / sprite.Texture.Size.X, y / sprite.Texture.Size.Y);
            defaultScale = new Vector2f(x, y);
        }

        public void SetPosition(float x, float y)
        {
            sprite.Position = new Vector2f(x, y);
        }

        public static Dictionary<string, Image> GetImages()
        {
            return images;
        }

        public void SetTexture(string nameTexture)
        {
            if (this.pathTexture.Equals(nameTexture + ".bubla")) return;
            this.pathTexture = nameTexture + ".bubla";
            Vector2f pos = sprite.Position;
            sprite = new Sprite();
            sprite.Texture = new Texture($"assets\\{nameTexture}.bubla");
            sprite.Scale = new Vector2f(defaultScale.X / sprite.Texture.Size.X, defaultScale.Y / sprite.Texture.Size.Y);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            sprite.Position = pos;
            //sprite.Scale = scale;
        }

        public Image CopyCreate(string name)
        {
            return new Image(name, pathTexture, sprite.Position - defaultScale / 2, defaultScale);
        }

        public void SetVeryUpLayer(bool value)
        {
            veryUpLayer = value;
        }
        
        public bool GetVeryUpLayer()
        {
            return veryUpLayer;
        }

        private void Dispose()
        {
            sprite.Dispose();
        }

        public static void Clear()
        {
            foreach (KeyValuePair<string, Image> listImage in images)
            {
                Image image = listImage.Value;
                image.Dispose();
            }
            images.Clear();
        }
    }
}
