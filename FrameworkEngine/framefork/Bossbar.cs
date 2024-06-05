using MyFramework.utils;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace Bubla
{
    public class Bossbar
    {
        private static Dictionary<string, Bossbar> bossbars = new Dictionary<string, Bossbar>();

        private RectangleShape background;
        private RectangleShape inside;
        private float count;
        private string name;
        private Vector2f position;
        private Vector2f scaleBackground;
        private Vector2f scaleInside;

        private bool active;

        public Bossbar() { }

        public Bossbar(string name, Vector2f position, Vector2f scale, Rgb outside, Rgb insideRgb)
        {
            this.name = name;
            this.count = 1;
            this.position = position;
            this.active = true;

            const float pixelSize = 0.26f;
            this.scaleBackground = new Vector2f(scale.X * pixelSize * 5, scale.Y * pixelSize * 5);
            this.scaleInside = new Vector2f(scale.X * pixelSize * 4.65f, scale.Y * pixelSize * 4);

            background = new RectangleShape();
            background.FillColor = new Color(outside.R, outside.G, outside.B, 255);
            background.Position = position;
            background.Size = this.scaleBackground;
            background.Origin = new Vector2f(this.scaleBackground.X / 2, this.scaleBackground.Y / 2);

            inside = new RectangleShape();
            inside.FillColor = new Color(insideRgb.R, insideRgb.G, insideRgb.B, 255);
            inside.Position = position;
            inside.Size = this.scaleInside;
            inside.Origin = new Vector2f(this.scaleInside.X / 2, this.scaleInside.Y / 2);

            Console.WriteLine(insideRgb.ToString());
            Console.WriteLine(scale);

            bossbars.Add(name, this);
        }

        public Bossbar Get(string name)
        {
            return bossbars[name];
        }

        public string Name
        {
            get { return name; }
        }

        public float GetCount()
        {
            return count;
        }

        public void SetCount(float count)
        {
            this.count = count;
            if (this.count < 0) this.count = 0;
            else if (this.count > 1) this.count = 1;
            inside.Size = new Vector2f(scaleInside.X * this.count, scaleInside.Y);
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public RectangleShape GetSpriteInside()
        {
            return inside;
        }

        public RectangleShape GetSpriteOutside()
        {
            return background;
        }

        public void AddPosition(float x, float y)
        {
            background.Position = new Vector2f(background.Position.X + x, background.Position.Y + y);
            inside.Position = new Vector2f(inside.Position.X + x, inside.Position.Y + y);
        }
        public void SetPosition(float x, float y)
        {
            background.Position = new Vector2f(x, y);
            inside.Position = new Vector2f(x, y);
        }

        public Vector2f Position
        {
            get { return position; }
            set { position = value; }
        }

        public static Dictionary<string, Bossbar> GetBossbars()
        {
            return bossbars;
        }

        private void Dispose()
        {
            background.Dispose();
            inside.Dispose();
        }

        public static void Clear()
        {
            foreach (KeyValuePair<string, Bossbar> listbossbar in bossbars)
            {
                Bossbar bossbar = listbossbar.Value;
                bossbar.Dispose();
            }
            bossbars.Clear();
        }
    }
}
