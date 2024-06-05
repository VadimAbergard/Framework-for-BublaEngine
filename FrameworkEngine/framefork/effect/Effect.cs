using MyFramework.framefork.effect;
using MyFramework.framefork.physics;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using test_Winforms.utils;

namespace Bubla
{
    public class Effect
    {
        private static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();

        private Sprite sprite;
        private string name;
        private string path;
        private Vector2f position;
        private float size;
        private int count;
        private TypeEffect typeEffect;

        private Particle[] particles;

        private int speed;
        private Timer timer;
        private bool playing;
        private bool active;
        
        private bool clone;

        public Effect() { }

        public Effect(string name, string path, Vector2f position, float size, int count, TypeEffect typeEffect, bool clone, int cloneSpeed, Position clonePosition)
        {
            timer = new Timer(0.01f);
            this.name = name;
            this.path = path;
            this.active = true;
            this.position = clonePosition == null ? position : new Vector2f(clonePosition.X, clonePosition.Y);
            this.size = size;
            this.count = count;
            this.clone = clone;
            this.typeEffect = typeEffect;
            if(cloneSpeed != -1) speed = cloneSpeed;
            //Console.WriteLine(cloneSpeed);

            /*sprite = new Sprite();
            sprite.Texture = new Texture("assets\\" + path);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            Vector2f scaleSprite = new Vector2f(size / sprite.Texture.Size.X, size / sprite.Texture.Size.Y);
            sprite.Scale = scaleSprite;
            position = new Vector2f(position.X + sprite.Texture.Size.X * sprite.Scale.X / 2f, position.Y + sprite.Texture.Size.Y * sprite.Scale.Y / 2f);
            sprite.Position = position;*/

            if(!clone) effects.Add(name, this);
            // test
            //Play(3);
        }

        public void Play(int speed = 1, Position pos = null)
        {
            this.speed = speed;
            playing = true;
            particles = new Particle[count];
            for (int i = 0; i < count; i++)
            {
                if (particles[i] != null)
                {
                    particles[i].Dispose();
                    particles[i] = null;
                }
                particles[i] = new Particle(path, position, size);
            }

            switch (typeEffect)
            {
                case TypeEffect.PULSE:
                    for (int i = 0; i < particles.Length; i++)
                    {
                        particles[i].Alpha = 255;
                        particles[i].UpdateVector();
                        particles[i].Position = pos == null ? position : new Vector2f(pos.X, pos.Y);
                    }
                    break;
            }

            if (clone) return;
            int id = 0;
            for (; ; id++)
            {
                if (!World.GetEffects().ContainsKey(id))
                {
                    World.GetEffects().Add(id, this.Clone(speed, pos));
                    break;
                }
            }
        }

        public void Update()
        {
            if (!active) return;
            timer.Add(-Game.SDelta() * speed);

            bool isParticlesLife = false;
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].Alpha == 0) continue;
                particles[i].GetSprite().Draw(Game.GetWindow(), RenderStates.Default);
                if (timer.GetFloat() < 0)
                {
                    particles[i].Alpha = (byte)(particles[i].Alpha - 1);
                }
                particles[i].Move();
                isParticlesLife = true;
            }
            if(timer.GetFloat() < 0) timer.Reset();
            if (!isParticlesLife)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Dispose();
                    particles[i] = null;
                }
                playing = false;
            }
        }

        public Effect Get(string name)
        {
            return effects[name];
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public string Name
        {
            get { return name; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public bool Playing
        {
            get { return playing; }
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public Vector2f Position
        {
            get { return position; }
            set { position = value; }
        }

        public void SetPosition(float x, float y)
        {
            position = new Vector2f(x, y);
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public TypeEffect TypeEffect
        {
            get { return typeEffect; }
            set { typeEffect = value; }
        }

        public static Dictionary<string, Effect> GetEffects()
        {
            return effects;
        }

        public Effect Clone(int cloneSpeed, Position clonePos)
        {
            return new Effect(name, path, Position, Size, count, TypeEffect, true, cloneSpeed, clonePos);
        }

        private void Dispose()
        {
            sprite.Dispose();
            for (int i = 0; i < count; i++)
            {
                if (particles[i] != null) particles[i].Dispose();
            }
        }
        public static void Clear()
        {
            foreach (KeyValuePair<string, Effect> listEffect in effects)
            {
                Effect effect = listEffect.Value;
                effect.Dispose();
            }
            effects.Clear();
        }
    }
}
