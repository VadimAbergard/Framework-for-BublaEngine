using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace Bubla
{

    public class World
    {
        private static Dictionary<string, GameObject> gameObjectsResourse = new Dictionary<string, GameObject>();
        private static Dictionary<int, GameObject> listGameObjects = new Dictionary<int, GameObject>();
        private static Dictionary<int, Effect> effects = new Dictionary<int, Effect>();
        private static List<int> ids = new List<int>();

        public static void Update()
        {
            foreach (KeyValuePair<int, GameObject> listGameObject in listGameObjects)
            {
                GameObject gameObject = listGameObject.Value;
                /*Console.WriteLine(gameObject.Name);
                Console.WriteLine("asdasd");*/
                gameObject.MouseOnGameObject = gameObject.GetSprite().GetGlobalBounds().Contains(Game.SCursorX(), Game.SCursorY());
                //Console.WriteLine("name: " + gameObject.Name + ", id: " + listGameObject.Key);
                if (gameObject.HasCollider && !gameObject.Trigger)
                {
                    float angle = gameObject.GetCollider().GetBody().GetAngle() * 180.0f / (float)Math.PI;
                    if(Game.Debug)
                    {
                        gameObject.GetCollider().Sprite.Position =
                                    new Vector2f(gameObject.GetCollider().GetBody().GetPosition().X,
                                    gameObject.GetCollider().GetBody().GetPosition().Y);

                        gameObject.GetCollider().Sprite.Rotation = angle;

                        gameObject.GetCollider().Sprite.Draw(Game.GetWindow(), RenderStates.Default);
                    }
                    gameObject.GetSprite().Rotation = angle;
                    gameObject.Position = new Vector2f(gameObject.GetCollider().GetPosition().X + gameObject.PosAddCollider.X * -1,
                                gameObject.GetCollider().GetPosition().Y + gameObject.PosAddCollider.Y);
                }
                gameObject.GetSprite().Draw(Game.GetWindow(), RenderStates.Default);

                // animation frame
                if (gameObject.AnimNow != null)
                {
                    gameObject.UpdateAnim();
                    if (gameObject.AnimNow.NextFrame)
                    {
                        gameObject.GetSprite().Texture = gameObject.AnimNow.FrameTexture;

                    }
                }
            }
            try
            {
                foreach (KeyValuePair<int, Effect> listEffects in effects)
                {
                    Effect effect = listEffects.Value;
                    if (!effect.Playing) effect.Play(effect.Speed, new Position(effect.Position.X, effect.Position.Y));
                    effect.Update();
                    if (!effect.Playing) effects.Remove(listEffects.Key);
                }
            } catch(InvalidOperationException e) { }
        }

        public int Spawn(string name, float x, float y)
        {
            GameObject gameObject = gameObjectsResourse[name].Clone(new Vector2f(x, y));
            return addGameObjectWorld(gameObject);
        }

        public int Spawn(string name, float x, float y, string newTag)
        {
            GameObject gameObject = gameObjectsResourse[name].Clone(new Vector2f(x, y), newTag);
            return addGameObjectWorld(gameObject);
        }

        public int Spawn(string name, float x, float y, string newTag, float sizeX, float sizeY)
        {
            GameObject gameObject = gameObjectsResourse[name].Clone(new Vector2f(x, y), newTag, sizeX, sizeY);
            return addGameObjectWorld(gameObject);
        }

        private int addGameObjectWorld(GameObject gameObject)
        {
            int id = 0;
            for (; ; id++)
            {
                if (!listGameObjects.ContainsKey(id))
                {
                    listGameObjects.Add(id, gameObject);
                    ids.Add(id);
                    break;
                }
            }
            return id;
        }

        public GameObject GetGameObject(int id)
        {
            return listGameObjects[id];
        }

        public static void Init()
        {
            foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
            {
                Console.WriteLine(listGameObject.Value.Resorse);
                if (listGameObject.Value.Resorse)
                {
                    gameObjectsResourse.Add(listGameObject.Value.Name, listGameObject.Value);
                }    
            }
        }

        public bool Kill(int id)
        {
            ids.Remove(id);
            listGameObjects[id].Dispose();
            return listGameObjects.Remove(id);
        }

        public static Dictionary<int, GameObject> GetGameObjects()
        {
            return listGameObjects;
        }

        public List<int> Ids()
        {
            return ids;
        }

        public static void Clear()
        {
            foreach (KeyValuePair<int, GameObject> listGameObject in listGameObjects)
            {
                GameObject gameObject = listGameObject.Value;
                gameObject.Dispose();
            }
            foreach (KeyValuePair<string, GameObject> listGameObject in gameObjectsResourse)
            {
                GameObject gameObject = listGameObject.Value;
                gameObject.Dispose();
            }
            listGameObjects.Clear();
            gameObjectsResourse.Clear();
            ids.Clear();
        }
        public static Dictionary<int, Effect> GetEffects()
        {
            return effects;
        }
    }
}
