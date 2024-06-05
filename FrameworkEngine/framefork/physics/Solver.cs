using Box2DX.Dynamics;
using Bubla;
using FrameworkEngine.utils;
using System;
using System.Collections.Generic;

namespace MyFramework.framefork.physics
{
    public class Solver : ContactListener
    {
        public override void Add(ContactPoint point)
        {
            base.Add(point);
            foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                foreach (KeyValuePair<string, GameObject> listGameObjectCollision in GameObject.GetGameObjects())
                {
                    GameObject gameObjectColission = listGameObjectCollision.Value;
                    if (gameObjectColission.Trigger) continue;
                    if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                    try
                    {
                        if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                        {
                            LuaScript.InvokeMethod("OnCollisionJoin", new Object[] { gameObject.Name, gameObjectColission.Name, -1 });
                            continue;
                        }
                    }
                    catch { }
                }
            }

            // world
            foreach (KeyValuePair<int, GameObject> listGameObject in Bubla.World.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                foreach (KeyValuePair<int, GameObject> listGameObjectCollision in Bubla.World.GetGameObjects())
                {
                    GameObject gameObjectColission = listGameObjectCollision.Value;
                    if (gameObjectColission.Trigger) continue;
                    if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                    try
                    {
                        if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                        {
                            LuaScript.InvokeMethod("OnCollisionJoin", new Object[] { gameObject.Name, gameObjectColission.Name, listGameObject.Key });
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }

        public override void Persist(ContactPoint point)
        {
            base.Persist(point);

            foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                foreach (KeyValuePair<string, GameObject> listGameObjectCollision in GameObject.GetGameObjects())
                {
                    GameObject gameObjectColission = listGameObjectCollision.Value;
                    if (gameObjectColission.Trigger) continue;
                    if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                    try
                    {
                        if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                        {
                            LuaScript.InvokeMethod("OnCollision", new Object[] { gameObject.Name, gameObjectColission.Name, -1 });
                            continue;
                        }
                    } catch { } 
                }
            }

            // world
            foreach (KeyValuePair<int, GameObject> listGameObject in Bubla.World.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                foreach (KeyValuePair<int, GameObject> listGameObjectCollision in Bubla.World.GetGameObjects())
                {
                    GameObject gameObjectColission = listGameObjectCollision.Value;
                    if (gameObjectColission.Trigger) continue;
                    if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                    try
                    {
                        if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                        {
                            LuaScript.InvokeMethod("OnCollision", new Object[] { gameObject.Name, gameObjectColission.Name, listGameObject.Key });
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }

        public override void Result(ContactResult point)
        {
            base.Result(point);
        }

        public override void Remove(ContactPoint point)
        {
            base.Remove(point);
            foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                foreach (KeyValuePair<string, GameObject> listGameObjectCollision in GameObject.GetGameObjects())
                {
                    GameObject gameObjectColission = listGameObjectCollision.Value;
                    if (gameObjectColission.Trigger) continue;
                    if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                    try
                    {
                        if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                        {
                            LuaScript.InvokeMethod("OnCollisionExit", new Object[] { gameObject.Name, gameObjectColission.Name, -1 });
                            continue;
                        }
                    }
                    catch { }
                }
            }

            // world
            foreach (KeyValuePair<int, GameObject> listGameObject in Bubla.World.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                foreach (KeyValuePair<int, GameObject> listGameObjectCollision in Bubla.World.GetGameObjects())
                {
                    GameObject gameObjectColission = listGameObjectCollision.Value;
                    if (gameObjectColission.Trigger) continue;
                    if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                    try
                    {
                        if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                        {
                            LuaScript.InvokeMethod("OnCollisionExit", new Object[] { gameObject.Name, gameObjectColission.Name, listGameObject.Key });
                            continue;
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
