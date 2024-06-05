using MyFramework.framefork.physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bubla
{
    public class GameObject
    {
        private static Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        private Dictionary<string, Animator> animation = new Dictionary<string, Animator>();
        private Animator animNow;
        private string nameAnimationNow;

        private Collider collider;

        private string name;
        private string tag;
        private float rotate;
        private string nameTexture;
        private bool point;
        private bool mouseOnGameObject;
        private Vector2f size;
        private Vector2f sizeNormal;
        private Vector2u defaultSizeTexture;
        private Vector2i splitSpriteSizeTexture;
        private Sprite sprite;

        private bool resorse;
        private bool smooth;
        private bool repited;

        private bool active;

        private bool hasCollider;
        private bool isInCollaider;
        private bool trigger;
        private bool fixedRotation;
        private bool fixedRotationTexture;
        private float angle;
        private float mass;
        private string typeCollider;

        // for clone
        private string animationText;
        private Vector2i splitSpriteSize;
        private Vector2i splitSpritePos;
        private bool colliderClone;
        private bool splitSprite;
        private bool splitSpritePixel;
        private Vector2i sizeAddCollider;
        private Vector2i posAddCollider;

        private int layer;

        private Position pos;
        private Position _size;

        public GameObject() { }

        public GameObject(string name, string tag, float x, float y, Vector2f size, Vector2i splitSpriteSize, string animation,
            Vector2i splitSpritePos, string nameTexture, bool point, bool resorse, bool collider,
            bool splitSprite, bool splitSpritePixel, bool trigger, float mass, Vector2i sizeAddCollider,
            Vector2i posAddCollider, int layer, bool fixedRotation, float angle, float rotate, bool smooth, bool repited,
            bool fixedRotationTexture, string typeCollider)
        {
            this.name = name;
            this.tag = tag;
            this.fixedRotation = fixedRotation;
            this.fixedRotationTexture = fixedRotationTexture;
            this.angle = angle;
            this.nameTexture = nameTexture;
            this.nameAnimationNow = "";
            this.point = point;
            this.rotate = rotate;
            this.layer = layer;
            this.typeCollider = typeCollider;
            this.splitSpriteSize = splitSpriteSize;
            this.splitSpritePos = splitSpritePos;
            this.colliderClone = collider;
            this.animationText = animation;
            this.splitSprite = splitSprite;
            this.splitSpritePixel = splitSpritePixel;
            this.sizeNormal = size;
            this.resorse = resorse;
            this.smooth = smooth;
            this.repited = repited;
            this.hasCollider = collider;
            this.sizeAddCollider = sizeAddCollider;
            this.posAddCollider = posAddCollider;
            this.trigger = trigger;
            this.mass = mass;
            this.active = true;

            if (nameTexture == "") return;
            sprite = new Sprite();
            if (!splitSprite) sprite.Texture = new Texture("assets\\" + nameTexture);
            else
            {
                if (splitSpritePixel)
                {
                    this.splitSpriteSizeTexture = new Vector2i(splitSpriteSize.X, splitSpriteSize.Y);
                    sprite.Texture = new Texture("assets\\" + nameTexture, new IntRect(splitSpritePos.X, splitSpritePos.Y, splitSpriteSizeTexture.X, splitSpriteSizeTexture.Y));
                }
                else
                {
                    this.defaultSizeTexture = new Texture("assets\\" + nameTexture).Size;
                    this.splitSpriteSizeTexture = new Vector2i((int)defaultSizeTexture.X / splitSpriteSize.X, (int)defaultSizeTexture.Y / splitSpriteSize.Y);
                    sprite.Texture = new Texture("assets\\" + nameTexture, new IntRect(splitSpriteSizeTexture.X * splitSpritePos.X, splitSpriteSizeTexture.Y * splitSpritePos.Y, splitSpriteSizeTexture.X, splitSpriteSizeTexture.Y));
                }
            }
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            this.size = new Vector2f(size.X / sprite.Texture.Size.X, size.Y / sprite.Texture.Size.Y);
            _size = new Position(this.size.X, this.size.Y);
            if (!repited)
            {
                sprite.Scale = this.size;
            } else
            {
                sprite.TextureRect = new IntRect(0, 0, (int)size.X, (int)size.Y);
                sprite.Origin = new Vector2f(size.X / 2f, size.Y / 2f);
                sprite.Texture.Repeated = true;
            }
            Vector2f position = new Vector2f(x + sprite.Texture.Size.X * sprite.Scale.X / 2f + (repited ? size.X / 2f : 0), y + sprite.Texture.Size.Y * sprite.Scale.Y / 2f + (repited ? size.Y / 2f : 0));
            sprite.Position = position;
            pos = new Position(position.X, position.Y);
            sprite.Rotation = rotate;
            if (smooth) sprite.Texture.Smooth = true;

            if (animation != "")
            {
                animation = animation.Remove(animation.Length - 1);
                animation = animation.Replace(" ", "");
                string[] textSplit = animation.Split(';');
                foreach (string text in textSplit)
                {
                    string[] splitText = text.Split(',');
                    List<int> frames = new List<int>();
                    for (int i = 0; ; i++)
                    {
                        try
                        {
                            frames.Add(int.Parse(splitText[3 + i]));
                        } catch { break; }
                    }
                    int[] framesArray = new int[frames.Count];
                    int k = 0;
                    foreach (int frame in frames)
                    {
                        framesArray[k++] = frame;
                    }
                    AddAnimator(splitText[0], float.Parse(splitText[1], CultureInfo.InvariantCulture),
                        splitText[2].Equals("true"), framesArray);
                }
            }

            if (collider)
            {
                float _angle = angle / 10f;
                this.collider = new Collider(new Vector2f(size.X + sizeAddCollider.X, size.Y + sizeAddCollider.Y),
                    new Vector2f(x + size.X / 2 + posAddCollider.X, y + size.Y / 2 - posAddCollider.Y), mass, fixedRotation, _angle, typeCollider, trigger);
                if(!fixedRotationTexture) sprite.Rotation = _angle;
            }
        }

        public GameObject Get(string name)
        {
            return gameObjects[name];
        }

        public string Name
        {
            get { return name; }
        }

        public string Tag()
        {
            return tag;
        }

        public void SetTag(string tag)
        {
            this.tag = tag;
        }

        public float Rotate()
        {
            return sprite.Rotation;
        }

        public void SetRotate(float rotate)
        {
            if (collider != null) return;
            sprite.Rotation = rotate;
        }

        public float GetSizeX()
        {
            return _size.X;
        }

        public float GetSizeY()
        {
            return _size.Y;
        }

        public Vector2f Position
        {
            get { return sprite.Position; }
            set {
                sprite.Position = value;
                pos = new Position(value.X, value.Y);
                if (collider != null) collider.SetPosition(new Vector2f(value.X + (trigger ? size.X / 2f : 0), value.Y + (trigger ? size.Y / 2f : 0)));
            }
        }

        public float GetPositionX()
        {
            return pos.X;
        }

        public float GetPositionY()
        {
            return pos.Y;
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetPos(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void SetColor(int r, int g, int b)
        {
            r = r > 255 ? 255 : r < 0 ? 0 : r;
            g = g > 255 ? 255 : g < 0 ? 0 : g;
            b = b > 255 ? 255 : b < 0 ? 0 : b;
            sprite.Color = new Color((byte)r, (byte)g, (byte)b);
        }

        public void SetAlphaColor(int alpha)
        {
            alpha = alpha > 255 ? 255 : alpha < 0 ? 0 : alpha;
            sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, (byte)alpha);
        }

        public void LookAt(float x, float y)
        {
            double dx = x - Position.X;
            double dy = y - Position.Y;
            float rotate = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
            sprite.Rotation = rotate;
        }

        public void SetTexture(string nameFile)
        {
            sprite.Texture = new Texture($"assets\\{nameFile}.bubla");
            sprite.Scale = new Vector2f(SizeNormal.X / sprite.Texture.Size.X, SizeNormal.Y / sprite.Texture.Size.Y);
            sprite.Origin = new Vector2f(sprite.Texture.Size.X / 2f, sprite.Texture.Size.Y / 2f);
            sprite.TextureRect = new IntRect(0, 0, (int)sprite.Texture.Size.X, (int)sprite.Texture.Size.Y);
            sprite.Texture.Smooth = smooth;
            sprite.Texture.Repeated = repited;
        }

        public void MoveToPointB(float x, float y, float speed) {
            
            float tempX = ((x - sprite.Position.X) / Distance(x - sprite.Position.X, y - sprite.Position.Y)) * speed;
            float tempY = ((y - sprite.Position.Y) / Distance(x - sprite.Position.X, y - sprite.Position.Y)) * speed;
            if (tempX > speed) tempX = speed;
            if (tempY > speed) tempY = speed;
            AddPosition(tempX , tempY);
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public bool IsInCollaider
        {
            get { return isInCollaider; }
            set { isInCollaider = value; } 
        }

        public bool FixedRotationTexture
        {
            get { return fixedRotationTexture; }
            set { fixedRotationTexture = value; } 
        }

        public void AddPositionTexture(float x, float y)
        {
            Vector2f vector = sprite.Position;
            vector.X += x;
            vector.Y += y;
            sprite.Position = vector;
        }

        public void SetScale(float x, float y)
        {
            this.sizeNormal = new Vector2f(x, y);
            this.size = new Vector2f(x / sprite.Texture.Size.X, y / sprite.Texture.Size.Y);
            sprite.Scale = this.size;
        }

        public Vector2f SizeNormal
        {
            get { return sizeNormal; }
        }

        public Vector2i PosAddCollider
        {
            get { return posAddCollider; }
        }

        public bool Resorse
        {
            get { return resorse; }
            set { resorse = value; }
        }

        public string NameTexture
        {
            get { return nameTexture; }
        }

        public void SetPosFrame(int row, int column)
        {
            sprite.Texture.Dispose();
            sprite.Texture = new Texture("assets\\" + nameTexture, new IntRect(splitSpriteSizeTexture.X * column, splitSpriteSizeTexture.Y * row, splitSpriteSizeTexture.X, splitSpriteSizeTexture.Y));
        }

        public void SetPosFramePixel(int x, int y, int sizeX, int sizeY)
        {
            sprite.Texture.Dispose();
            sprite.Texture = new Texture("assets\\" + nameTexture, new IntRect(x, y, sizeX, sizeY));
        }

        public void AddAnimator(string nameAnimation, float speed, bool loop, params int[] framePos)
        {
            animation.Add(nameAnimation, new Animator(framePos, speed, loop, nameTexture, splitSpriteSizeTexture, smooth));
        }

        public void PlayAnimation(string nameAnimation)
        {
            if (nameAnimationNow.Equals(nameAnimation))
            {
                animNow.PauseAnim = false;
                return;
            }

            animNow = animation[nameAnimation];
            nameAnimationNow = nameAnimation;
        }

        public void PauseAnimation()
        {
            animNow.Pause();
        }

        public bool IsPauseAnimation()
        {
            return animNow.PauseAnim;
        }

        public void ResumeAnimation()
        {
            animNow.Resume();
        }

        public void FuturePause(string nameAnimation, int frame)
        {
            animation[nameAnimation].AddFuturePause(frame);
        }

        public void RemoveFuturePause(string nameAnimation, int frame)
        {
            animation[nameAnimation].RemoveFuturePause(frame);
        }

        internal void UpdateAnim()
        {
            animNow.Update();
        }

        public void SetPosition(float x, float y)
        {
            if(hasCollider)
            {
                collider.GetBody().SetXForm(new Box2DX.Common.Vec2(x, y), angle);
                return;
            }
            sprite.Position = new Vector2f(x, y);
        }

        public float Distance(GameObject gameObject)
        {
            return (float)Math.Sqrt(Math.Pow((gameObject.GetSprite().Position.X - sprite.Position.X), 2) + Math.Pow((gameObject.GetSprite().Position.Y - sprite.Position.Y), 2));
        }

        public float Distance(float x, float y)
        {
            return (float)Math.Sqrt(Math.Pow((x - sprite.Position.X), 2) + Math.Pow((y - sprite.Position.Y), 2));
        }

        public void MirrorX(bool mirror)
        {
            if (mirror && sprite.Scale.X > 0) MirrorTexture(true);
            else if (!mirror && sprite.Scale.X < 0) MirrorTexture(true);
        }

        public void MirrorY(bool mirror)
        {
            if (mirror && sprite.Scale.Y > 0) MirrorTexture(false);
            else if (!mirror && sprite.Scale.Y < 0) MirrorTexture(false);
        }

        private void MirrorTexture(bool isX)
        {
            Vector2f vector = sprite.Scale;
            if(isX) vector.X *= -1;
            else vector.Y *= -1;
            sprite.Scale = vector;
        }

        internal Animator AnimNow
        {
            get { return animNow; }
        }

        public bool Point
        {
            get { return point; }
        }

        public bool Trigger
        {
            get { return trigger; }
        }

        public bool SplitSprite
        {
            get { return splitSprite; }
            set { splitSprite = value; }
        }

        public bool SplitSpritePixel
        {
            get { return splitSpritePixel; }
            set { splitSpritePixel = value; }
        }

        public float GetMass()
        {
            return mass;
        }

        public void SetMass(float mass)
        {
            this.mass = mass;
        }

        public int Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public bool HasCollider
        {
            get { return hasCollider; }
        }

        public bool IsMouseOnGameObject()
        {
            return mouseOnGameObject;
        }

        public bool MouseOnGameObject
        {
            set { mouseOnGameObject = value; }
        }

        public Collider GetCollider()
        {
            return collider;
        }

        public static Dictionary<string, GameObject> GetGameObjects()
        {
            return gameObjects;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public void AddPosition(float x, float y)
        {
            if (!active) return;
            sprite.Position = new Vector2f(sprite.Position.X + x, sprite.Position.Y + y);
        }

        public void Clone(string name, float x, float y) // for lua script
        {
            gameObjects.Add(name, new GameObject(name, tag, x, y, sizeNormal,
                splitSpriteSize, animationText, splitSpritePos, nameTexture, point, resorse, colliderClone,
                splitSprite, splitSpritePixel, trigger, mass, sizeAddCollider, posAddCollider, layer, fixedRotation, angle, rotate, smooth, repited,
                fixedRotationTexture, typeCollider));
        }

        public GameObject Clone(Vector2f newPosition, string newTag = "", float sizeX = -1, float sizeY = -1)
        {
            return new GameObject(name, newTag == "" ? tag : newTag, newPosition.X, newPosition.Y, sizeX == -1 && sizeY == -1 ? sizeNormal : new Vector2f(sizeX, sizeY),
                splitSpriteSize, animationText, splitSpritePos, nameTexture, point, resorse, colliderClone, 
                splitSprite, splitSpritePixel, trigger, mass, sizeAddCollider, posAddCollider, layer, fixedRotation, angle, rotate, smooth, repited,
                fixedRotationTexture, typeCollider);
        }

        public void Impulse(float x, float y)
        {
            collider.Impulse(x, y);
        }

        public void ImpulseX(float x)
        {
            collider.ImpulseX(x);
        }

        public void ImpulseY(float y)
        {
            collider.ImpulseY(y);
        }

        public void ApplyImpulse(float x, float y)
        {
            collider.ApplyImpulse(x, y);
        }

        public void StopCollider()
        {
            collider.Stop();
        }
        
        public void Dispose()
        {
            sprite.Dispose();
            if(animNow != null) animNow.Dispose();
            if(collider != null) collider.Dispose();
        }
        public static void Clear()
        {
            foreach(KeyValuePair<string, GameObject> listGameObject in gameObjects)
            {
                GameObject gameObject = listGameObject.Value;
                gameObject.Dispose();
            }
            gameObjects.Clear();
        }

        public bool GetActiveCollider()
        {
            return collider.Active;
        }

        public void SetActiveCollider(bool active)
        {
            collider.Active = active;
        }
    }
}
