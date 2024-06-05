using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SFML.Window.Mouse;
using static System.Net.Mime.MediaTypeNames;

namespace Bubla
{
    public class Button
    {
        private static List<Button> buttons = new List<Button>();

        private Sprite sprite;
        private string name;
        private string pathTexture;
        private Vector2f position;
        private Vector2f scale;

        private bool active;

        public Button() { }

        public Button(string name, string path, Vector2f position, Vector2f scale) {
            this.name = name;
            this.position = position;
            this.pathTexture = path;
            this.active = true;
            sprite = new Sprite();
            sprite.Texture = new Texture("assets\\" + pathTexture);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            this.scale = scale;
            Vector2f scaleSprite = new Vector2f(scale.X / sprite.Texture.Size.X, scale.Y / sprite.Texture.Size.Y);
            sprite.Scale = scaleSprite;
            position = new Vector2f(position.X + sprite.Texture.Size.X * sprite.Scale.X / 2f, position.Y + sprite.Texture.Size.Y * sprite.Scale.Y / 2f);
            sprite.Position = position;
        }

        public Button CopyCreate(string name)
        {
            Button newButton = new Button(name, pathTexture, position, scale);
            Button.GetButtons().Add(newButton);
            return newButton;
        }

        public bool Click()
        {
            return sprite.GetGlobalBounds().Contains(Game.SCursorX(), Game.SCursorY()) && active;
        }

        public Button Get(string name)
        {
            foreach (Button button in buttons)
            {
                if (button.Name.Equals(name))
                {
                    return button;
                }
            }
            return null;
        }

        public string Name
        {
            get { return name; }
        }

        public void SetTexture(string nameTexture)
        {
            if (this.pathTexture.Equals(nameTexture + ".bubla")) return;
            this.pathTexture = nameTexture + ".bubla";
            sprite = new Sprite();
            sprite.Texture = new Texture($"assets\\{nameTexture}.bubla");
            sprite.Scale = new Vector2f(scale.X / sprite.Texture.Size.X, scale.Y / sprite.Texture.Size.Y);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            sprite.Position = position; 
        }

        public float GetPositionX()
        {
            return sprite.Position.X;
        }

        public float GetPositionY()
        {
            return sprite.Position.Y;
        }

        public void SetColor(int r, int g, int b, int a)
        {
            sprite.Color = new Color((byte)r, (byte)g, (byte)b, (byte)a);
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public float GetSizeX()
        {
            return scale.X;
        }

        public float GetSizeY()
        {
            return scale.Y;
        }

        public Position GetPosition()
        {
            return new Position(position.X, position.Y);
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public void AddPosition(float x, float y)
        {
            position = new Vector2f(sprite.Position.X + x - sprite.Texture.Size.X * sprite.Scale.X / 2f, sprite.Position.Y + y - sprite.Texture.Size.Y * sprite.Scale.Y / 2f);
            sprite.Position = new Vector2f(sprite.Position.X + x + sprite.Scale.X / 2f, sprite.Position.Y + y + sprite.Scale.Y / 2f);
        }

        public void SetPosition(float x, float y)
        {
            position = new Vector2f(x - sprite.Texture.Size.X * sprite.Scale.X / 2f, y - sprite.Texture.Size.Y * sprite.Scale.Y / 2f);
            sprite.Position = new Vector2f(x + sprite.Scale.X / 2f, y + sprite.Scale.Y / 2f);
        }

        public static List<Button> GetButtons()
        {
            return buttons;
        }
        private void Dispose()
        {
            sprite.Dispose();
        }

        public static void Clear()
        {
            foreach (Button button in buttons)
            {
                button.Dispose();
            }
            buttons.Clear();
        }
    }
}
