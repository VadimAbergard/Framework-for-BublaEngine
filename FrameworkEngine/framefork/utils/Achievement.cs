using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Bubla
{
    public class Achievement
    {

        private readonly static Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
        private static SFML.Audio.Music sound;
        private readonly string name;
        private readonly string headText;
        private readonly string lore;
        private readonly Sprite sprite;

        //private static bool complete;
        //private static string nameComplete;

        public Achievement() { }

        public Achievement(string name, string nameTexture, string headText, string lore)
        {
            this.name = name;
            this.headText = headText;
            this.lore = lore;
            sprite = new Sprite();
            try
            {
                sprite.Texture = new Texture($"assets\\{nameTexture}.png");
            }
            catch { sprite.Texture = new Texture($"assets\\{nameTexture}.bubla"); }
            sprite.Position = new Vector2f(5000, 5000);
            sprite.Scale = new Vector2f(3, 3);
            sprite.Origin = new Vector2f(sprite.Scale.X / 2, sprite.Scale.Y / 2);
        }

        public static Achievement Get(string name)
        {
            return achievements[name];
        }

        public string Name { get => name; }
        public Sprite Sprite { get => sprite; }
        public string HeadText { get => headText; }
        public string Lore { get => lore; }
        //public static bool Complet { get => complete; }
        public void Draw(float x, float y, float scale)
        {
            if (Game.AnimAcivmentNow) return;
            sprite.Position = new Vector2f(x, y);
            sprite.Scale = new Vector2f(scale, scale);
            sprite.Origin = new Vector2f(sprite.Scale.X / 2, sprite.Scale.Y / 2);
            sprite.Draw(Game.GetWindow(), RenderStates.Default);
            sprite.Position = new Vector2f(5000, 5000);
            sprite.Scale = new Vector2f(3, 3);
            sprite.Origin = new Vector2f(sprite.Scale.X / 2, sprite.Scale.Y / 2);
        }
        public static Dictionary<string, Achievement> AchievementsList { get => achievements; }

        public static void Create(string name, string nameTexture, string headText, string lore)
        {
            achievements.Add(name, new Achievement(name, nameTexture, headText, lore));
        }

        public static void Complete(string name) 
        {
            achievements[name].Sprite.Position = new Vector2f(Game.GetCameraUI().Size.X - 370 /*- achievements[name].Sprite.Scale.X*/, Game.GetCameraUI().Size.Y + 30);
            //achievements[name].Sprite.Position = new Vector2f(100, 100);
            Game.AnimationAchivcment(name, achievements[name].HeadText, achievements[name].Lore);
        }

        public static void SetSound(string nameSound, float volume)
        {
            if (sound != null) sound.Dispose();
            try
            {
                sound = new SFML.Audio.Music($"assets\\{nameSound}.ogg");
            }
            catch { sound = new SFML.Audio.Music($"assets\\{nameSound}.bubla"); }
            sound.Volume = volume;
        }

        public static void PlaySound()
        {
            if (sound != null) sound.Play();
        }
    }
}
