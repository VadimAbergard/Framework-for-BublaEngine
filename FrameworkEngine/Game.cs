
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SFML.System;
using SFML.Window;
using System.Globalization;
using static SFML.Window.Sensor;
using Type = System.Type;
using MyFramework.framefork.physics;
using System.Security.Policy;
using test_Winforms.utils;
using MyFramework.utils;
using System.Text;
using System.Xml.Linq;
using System.Threading;
using static SFML.Window.Mouse;
using static System.Net.Mime.MediaTypeNames;
using static SFML.Window.Keyboard;
using System.Diagnostics;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using LuaNET.Lua54;
using LuaInterface;
using Lua = LuaInterface.Lua;
using System.Drawing;
using FrameworkEngine.utils;
using System.Speech.Synthesis;
using Math = System.Math;
using FrameworkEngine.framefork.world;
using System.Data.Common;

namespace Bubla
{
    public class Game
    {
        private static float width;
        private static float height;

        private static string titleGame;
        private static uint widthScreen;
        private static uint heightScreen;
        private static uint defailtWidthScreen = 728;
        private static uint defailtHeightScreen = /*485*/(uint)(defailtWidthScreen / 1.5f);
        private static bool debug;
        private static bool deviceConnect;

        private static bool consoleVisible;
        private static RectangleShape consoleTexture;
        private static SFML.Graphics.Text consoleText;


        private static float delta;
        private static float fps;

        private static byte[] colorBackgroundRGB;
        private static Random random = new Random();

        private static bool keyPressed = false;
        private static bool initObjects = true;

        private static bool windowCreate = false;
        private static RenderWindow window;

        private static View camera;
        private static View cameraUI;

        private static List<string> pathsGlobalScript = new List<string>();

        private static Box2DX.Dynamics.World world;

        private static float sizeMouseTexture = 0;
        private static bool mouseInGame;

        private static RectangleShape backgroundAcivment;
        private static Timer timerAcivment = new Timer(3);
        private static bool animAcivmentNow = false;
        private static string nameObjectAchivment = "";
        private static SFML.Graphics.Text headTextAchivment;
        private static SFML.Graphics.Text loreAchivment;
        private const float speedAnimationAchivment = 2.2f;

        private static void Main(string[] args)
        {
            try
            {
                // acivments init
                backgroundAcivment = new RectangleShape();
                backgroundAcivment.FillColor = new SFML.Graphics.Color(0, 0, 0); // 30, 30, 60
                backgroundAcivment.Position = new Vector2f(400, 400);
                backgroundAcivment.Size = new Vector2f(400, 120);
                backgroundAcivment.Origin = new Vector2f(backgroundAcivment.Size.X / 2, backgroundAcivment.Size.Y / 2);

                headTextAchivment = new SFML.Graphics.Text();
                try
                {
                    headTextAchivment.Font = new Font("assets\\font.ttf");
                }
                catch { headTextAchivment.Font = new Font("assets\\font.bubla"); }
                headTextAchivment.CharacterSize = 35;
                headTextAchivment.Position = new Vector2f(5000, 5000);
                headTextAchivment.Color = new SFML.Graphics.Color(255, 252, 137);

                loreAchivment = new SFML.Graphics.Text();
                try
                {
                    loreAchivment.Font = new Font("assets\\font.ttf");
                }
                catch { loreAchivment.Font = new Font("assets\\font.bubla"); }
                loreAchivment.CharacterSize = 25;
                loreAchivment.Position = new Vector2f(5000, 5000);
                loreAchivment.Color = new SFML.Graphics.Color(129, 129, 129);

                // world init
                AABB aabb = new AABB();
                aabb.LowerBound.Set(-100000, -100000); // Указываем левый верхний угол начала границ
                aabb.UpperBound.Set(100000, 100000); // Указываем нижний правый угол конца границ
                Vec2 g = new Vec2(0, 0); // Устанавливаеи вектор гравитации
                world = new Box2DX.Dynamics.World(aabb, g, false); // Создаем мир

                SSetScene(null);

                if (debug)
                {
                    consoleTexture = new RectangleShape();
                    consoleTexture.OutlineThickness = 2;
                    consoleTexture.OutlineColor = SFML.Graphics.Color.White;
                    consoleTexture.FillColor = new SFML.Graphics.Color(0, 0, 0, 200);
                    consoleTexture.Size = new Vector2f(SFML.Window.VideoMode.DesktopMode.Width, 100);
                    consoleText = new SFML.Graphics.Text();
                    try
                    {
                        consoleText.Font = new Font("assets\\font.ttf");
                    }
                    catch { consoleText.Font = new Font("assets\\font.bubla"); }
                    consoleText.CharacterSize = 60;
                    consoleText.Position = new Vector2f(5, 9);
                }


                initObjects = false;

                if (deviceConnect)
                {
                    Device.Connect();
                }

                foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
                {
                    GameObject gameObject = listGameObject.Value;
                }

                window = new RenderWindow(new SFML.Window.VideoMode((uint)width, (uint)height), titleGame, !debug ? Styles.Fullscreen : Styles.Close);
                window.SetView(camera);

                string path = AppDomain.CurrentDomain.BaseDirectory + "assets";

                window.Closed += (s, e) =>
                {
                    bool exitProgramm = true;

                    try
                    {
                        exitProgramm = (bool)LuaScript.InvokeMethod("onExit")[0];
                    }
                    catch (NullReferenceException ex) { }

                    if (!exitProgramm) return;

                    DateTime dateTimeExit = DateTime.Now;
                    Save.SWrite("offline", $"{dateTimeExit.Year},{dateTimeExit.Month},{dateTimeExit.Day},{dateTimeExit.Hour},{dateTimeExit.Minute}");
                    window.Close();
                };
                window.KeyPressed += (s, e) =>
                {
                    if (keyPressed) return;
                    keyPressed = true;
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Home) && debug) {
                        consoleVisible = !consoleVisible;
                        return;
                    } else if (consoleVisible) {
                        string key = e.Code.ToString().ToLower().Replace("num", "");
                        if (key.Equals("left") || key.Equals("right") || key.Equals("up") || key.Equals("down")) return;
                        if(key.Equals("return"))
                        {
                            LuaScript.InvokeMethod("onConsole", new object[] { consoleText.DisplayedString.Split(' ') });

                            consoleVisible = false;
                            consoleText.DisplayedString = "";
                            return;
                        }
                        else if (key.Equals("backspace"))
                        {
                            if (consoleText.DisplayedString.Length == 0) return;
                            consoleText.DisplayedString = consoleText.DisplayedString.Remove(consoleText.DisplayedString.Length - 1);
                            return;
                        }
                        else if (key.Equals("space"))
                        {
                            consoleText.DisplayedString += " ";
                            return;
                        }
                        consoleText.DisplayedString += key;
                        return;
                    }
                    LuaScript.InvokeMethod("KeyDown", new Object[] { e.Code.ToString().ToLower() });
                };
                window.KeyReleased += (s, e) =>
                {
                    keyPressed = false;
                    LuaScript.InvokeMethod("KeyUp", new Object[] { e.Code.ToString() });
                };
                window.Resized += (s, e) =>
                {
                    camera = new View(new FloatRect(0, 0, e.Width, e.Height));
                    cameraUI = new View(new FloatRect(0, 0, e.Width, e.Height));
                };
                window.MouseButtonPressed += (s, e) =>
                {
                    foreach (Button button in Button.GetButtons())
                    {
                        if (button.Click())
                        {
                            LuaScript.InvokeMethod("OnButtonClick", new Object[] { button.Name });
                            break;
                        }
                    }
                };
                window.MouseWheelScrolled += (s, e) =>
                {
                    LuaScript.InvokeMethod("OnMouseWheelScrolled", new Object[] { e.Delta });
                };
                window.GainedFocus += (s, e) =>
                {
                    LuaScript.InvokeMethod("OnFocusWindow");
                };
                window.LostFocus += (s, e) =>
                {
                    LuaScript.InvokeMethod("OnUnFocusWindow");
                };
                window.MouseButtonPressed += (s, e) =>
                {
                    LuaScript.InvokeMethod("OnMousePressed", new Object[] { e.Button.ToString() });
                };
                window.MouseButtonReleased += (s, e) =>
                {
                    LuaScript.InvokeMethod("OnMouseReleased", new Object[] { e.Button.ToString() });
                };
                Texture icon = null;
                try
                {
                    try
                    {
                        icon = new Texture("assets\\icon.bubla");
                    }
                    catch { icon = new Texture("assets\\icon.png"); };
                }
                catch { }
                if(icon != null) window.SetIcon(icon.Size.X, icon.Size.Y, icon.CopyToImage().Pixels);
                window.SetFramerateLimit(60);
                SFML.Graphics.Color color = new SFML.Graphics.Color(colorBackgroundRGB[0], colorBackgroundRGB[1], colorBackgroundRGB[2]);

                Sprite textureCursor = null;
                if(!mouseInGame)
                {
                    window.SetMouseCursorVisible(false);
                }
                else if (File.Exists("assets\\mouse.png") || File.Exists("assets\\mouse.bubla"))
                {
                    window.SetMouseCursorVisible(false);
                    textureCursor = new Sprite();
                    try
                    {
                        textureCursor.Texture = new Texture("assets\\mouse.png");
                    } catch { textureCursor.Texture = new Texture("assets\\mouse.bubla"); }
                    textureCursor.Scale = new Vector2f(sizeMouseTexture, sizeMouseTexture);
                    Console.WriteLine($"sizeMouseTexture {sizeMouseTexture}");
                }

                Clock clock = new Clock();
                Timer timerUpdateFps = new Timer(1);

                windowCreate = true;

                while (window.IsOpen)
                {
                    window.DispatchEvents();

                    window.Clear(color);

                    delta = clock.Restart().AsSeconds();

                    world.Step(1f, 6, 2);
                    timerUpdateFps.Add(-delta);
                    if (timerUpdateFps.GetFloat() <= 0) {
                        fps = 1.0f / delta;
                        timerUpdateFps.Reset();
                    }
                    if (debug) window.SetTitle($"{titleGame} | fps: {(int)fps}");

                    window.SetView(camera);

                    // mouse update
                    if (textureCursor != null)
                    {
                        textureCursor.Position = new Vector2f(SCursorX(), debug ? SCursorY() - 60 : SCursorY());
                        textureCursor.Draw(window, RenderStates.Default);
                    }

                    foreach (KeyValuePair<string, Sound> listSounds in Sound.GetSounds())
                    {
                        Sound sound = listSounds.Value;

                        if(sound.IsPlaying() && sound.TimeAttenuation != 0)
                        {
                            if (sound.Volume - sound.TimeAttenuation * delta * 100 < 0)
                            {
                                sound.Volume = 0;
                                sound.GetSound.Stop();
                                sound.TimeAttenuation = 0;
                                continue;
                            }
                            sound.GetSound.Volume -= sound.TimeAttenuation * delta * 100;
                            continue;
                        }

                        if (!sound.IsPlaying() || sound.TimeAddition == 0) continue;
                        if(sound.Volume + sound.TimeAddition * delta * 100 > sound.VolumeAddition)
                        {
                            sound.Volume = sound.VolumeAddition;
                            sound.TimeAddition = 0;
                            continue;
                        }
                        sound.GetSound.Volume += sound.TimeAddition * delta * 100;
                    }

                    // draw tiled map
                    foreach (Sprite sprite in TiledMap.GetSpites())
                    {
                        if (sprite.Texture == null) continue;
                        if (Distance(sprite.Position, camera.Center) < 2000) sprite.Draw(window, RenderStates.Default);
                    }
                    if (debug) {
                        foreach (Collider collider in TiledMap.GetColliders())
                        {
                            collider.Sprite.Position = new Vector2f(collider.GetBody().GetPosition().X,
                                        collider.GetBody().GetPosition().Y);
                            collider.Sprite.Draw(window, RenderStates.Default);
                        }
                    }

                    // draw objects
                    World.Update();

                    foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
                    {
                        GameObject gameObject = listGameObject.Value;
                        if (gameObject.Point || gameObject.Resorse || !gameObject.GetActive() || gameObject.GetSprite() == null) continue;
                        if (Distance(gameObject.Position, camera.Center) < 2000) gameObject.GetSprite().Draw(window, RenderStates.Default);
                        // other check
                        gameObject.MouseOnGameObject = gameObject.GetSprite().GetGlobalBounds().Contains(SCursorX(), SCursorY());
                        
                        // animation frame
                        if (gameObject.AnimNow != null)
                        {
                            gameObject.UpdateAnim();
                            if (gameObject.AnimNow.NextFrame)
                            {
                                gameObject.GetSprite().Texture = gameObject.AnimNow.FrameTexture;
                            }
                        }
                        // collider
                        if (gameObject.HasCollider)
                        {
                            float angle = 0;
                            if(!gameObject.Trigger) angle = gameObject.GetCollider().GetBody().GetAngle() * 180.0f / (float)System.Math.PI;
                            if (gameObject.GetCollider().Sprite != null) {
                                if (!gameObject.Trigger)
                                {
                                    gameObject.GetCollider().Sprite.Position =
                                    new Vector2f(gameObject.GetCollider().GetBody().GetPosition().X,
                                    gameObject.GetCollider().GetBody().GetPosition().Y);

                                    gameObject.GetCollider().Sprite.Rotation = angle;
                                }
                                gameObject.GetCollider().Sprite.Draw(window, RenderStates.Default);
                            }
                            if (!gameObject.Trigger)
                            {
                                if (!gameObject.FixedRotationTexture) gameObject.GetSprite().Rotation = angle;
                                gameObject.Position = new Vector2f(gameObject.GetCollider().GetPosition().X + gameObject.PosAddCollider.X * -1,
                                    gameObject.GetCollider().GetPosition().Y + gameObject.PosAddCollider.Y);
                            }

                            // check collision trigger
                            foreach (KeyValuePair<string, GameObject> listGameObjectCollision in GameObject.GetGameObjects())
                            {
                                GameObject gameObjectColission = listGameObjectCollision.Value;
                                if (!gameObjectColission.Trigger) continue;
                                if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                                if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                                {
                                    if (gameObjectColission.IsInCollaider) {
                                        LuaScript.InvokeMethod("OnCollision", new Object[] { gameObject.Name, gameObjectColission.Name, -1 });
                                    }
                                    else {
                                        gameObjectColission.IsInCollaider = true;
                                        LuaScript.InvokeMethod("OnCollisionJoin", new Object[] { gameObject.Name, gameObjectColission.Name, -1 });
                                    }
                                    continue;
                                } else if(gameObjectColission.IsInCollaider)
                                {
                                    gameObjectColission.IsInCollaider = false;
                                    LuaScript.InvokeMethod("OnCollisionExit", new Object[] { gameObject.Name, gameObjectColission.Name, -1 });
                                }
                            }
                            // world
                            foreach (KeyValuePair<int, GameObject> listGameObjectCollision in World.GetGameObjects())
                            {
                                GameObject gameObjectColission = listGameObjectCollision.Value;
                                if (!gameObjectColission.HasCollider || gameObject == gameObjectColission) continue;
                                if (gameObject.GetCollider().Sprite.GetGlobalBounds().Intersects(gameObjectColission.GetCollider().Sprite.GetGlobalBounds()))
                                {
                                    if (gameObjectColission.IsInCollaider)
                                    {
                                        LuaScript.InvokeMethod("OnCollision", new Object[] { gameObject.Name, gameObjectColission.Name, listGameObjectCollision.Key });
                                    }
                                    else
                                    {
                                        gameObjectColission.IsInCollaider = true;
                                        try
                                        {
                                            LuaScript.InvokeMethod("OnCollisionJoin", new Object[] { gameObject.Name, gameObjectColission.Name, listGameObjectCollision.Key });
                                        }
                                        catch { }
                                    }
                                    continue;
                                } else if (gameObjectColission.IsInCollaider)
                                {
                                    gameObjectColission.IsInCollaider = false;
                                    LuaScript.InvokeMethod("OnCollisionExit", new Object[] { gameObject.Name, gameObjectColission.Name, listGameObjectCollision.Key });
                                }
                            }
                        }
                            
                    window.SetView(cameraUI);

                    foreach (KeyValuePair<string, Bossbar> bossbar in Bossbar.GetBossbars())
                    {
                        if (!bossbar.Value.GetActive()) continue;
                        bossbar.Value.GetSpriteOutside().Draw(window, RenderStates.Default);
                        bossbar.Value.GetSpriteInside().Draw(window, RenderStates.Default);
                    }

                    foreach (KeyValuePair<string, Image> image in Image.GetImages())
                    {
                        if (!image.Value.GetActive() || image.Value.GetVeryUpLayer()) continue;
                        image.Value.GetSprite().Draw(window, RenderStates.Default);
                    }

                    foreach (Button button in Button.GetButtons())
                    {
                        if (!button.GetActive()) continue;
                        button.GetSprite().Draw(window, RenderStates.Default);
                    }

                    foreach (KeyValuePair<string, Text> text in Text.GetTexts())
                    {
                        if (!text.Value.GetActive() || text.Value.GetVeryUpLayer()) continue;
                        text.Value.SfmlText.Draw(window, RenderStates.Default);
                    }

                    //VeryUpLayer
                    foreach (KeyValuePair<string, Image> image in Image.GetImages())
                    {
                        if (!image.Value.GetActive() || !image.Value.GetVeryUpLayer()) continue;
                        image.Value.GetSprite().Draw(window, RenderStates.Default);
                    }

                    foreach (KeyValuePair<string, Text> text in Text.GetTexts())
                    {
                        if (!text.Value.GetActive() || !text.Value.GetVeryUpLayer()) continue;
                        text.Value.SfmlText.Draw(window, RenderStates.Default);
                    }

                    // console 
                    if (consoleVisible)
                    {
                        consoleTexture.Draw(window, RenderStates.Default);
                        consoleText.Draw(window, RenderStates.Default);
                    }
                    else
                    {
                        LuaScript.InvokeMethod("Update", new Object[] { delta });
                    }

                    window.Display();

                    if(nameSceneNext != "")
                    {
                        if (sceneNext) return;
                        sceneNext = true;
                        addAllGameObjects(nameSceneNext);
                        World.Init();
                        LuaScript.InvokeMethod("Start");
                        sceneNext = false;
                        nameSceneNext = "";
                    }
                }
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.White;

                string newLine = "\n";
                string[] randomText = { "упс что-то пошло не так!", "пжж, пжж, ошибка, ошибка.", "и... Бам!!!", "игра завершилась с ошибкой!", "фаталити ерор!", "жаль что игре пришлось закрытся :("};
                string textWrite = $"{randomText[new Random().Next(randomText.Length)]}|текст ниже, отправте разработчику!{newLine}text below send this to the developer!{newLine + newLine}---------------------------------------------------{newLine} {e.ToString()}";

                int freeId = 0;
                while(File.Exists("error" + freeId + ".txt"))
                {
                    freeId++;
                }
                string path = "error" + freeId + ".txt";

                FileStream fsWrite = new FileStream(path, FileMode.Create);

                byte[] bytesWrite = Encoding.UTF8.GetBytes(textWrite);
                string finalText = "";
                for (int i = 0; i < bytesWrite.Length; i++)
                {
                    finalText += bytesWrite[i];
                    fsWrite.WriteByte(bytesWrite[i]);
                }
                fsWrite.Close();


                textWrite = textWrite.Replace(" ", "_").Replace("|", " ");

                Process myProcess = new Process();
                myProcess.StartInfo.FileName = "lib\\window\\ErrorWindow.exe"; //not the full application path
                myProcess.StartInfo.Arguments = textWrite;
                myProcess.Start();

                CustomError();
            }
        }

        private static void addAllGameObjects(string nameScene)
        {
            XmlDocument xDoc = new XmlDocument();
            string pathXmlBubla = "assets\\infoGame.bubla";
            string pathXml = "assets\\infoGame.xml";
            try
            {
                xDoc.Load(new StringReader(Encryption.DencrypText(File.ReadAllText(pathXmlBubla))));
            } catch (FileNotFoundException e) { xDoc.Load(new StringReader(Encryption.DencrypText(File.ReadAllText(pathXml)))); }

            bool sceneLoad = true;
            
            GameObject.Clear();
            Effect.Clear();
            Button.Clear();
            Image.Clear();
            Bossbar.Clear();
            Sound.Clear();
            World.Clear();
            Text.Clear();
            LuaScript.Dispose();
            

            XmlElement xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                string tag = "";
                string nameTexture = "";
                float x = 0;
                float y = 0;
                float sizeX = 0;
                float sizeY = 0;
                float rotate = 0;
                bool point = false;
                bool resourse = false;
                bool smooth = false;
                bool repited = false;

                bool collider = false;
                bool trigger = false;
                bool fixedRotation = false;
                bool fixedRotationTexture = false;
                float angle = 0;
                float mass = 0;
                string typeCollider = "square";
                Vector2i sizeAddCollider = new Vector2i();
                Vector2i posAddCollider = new Vector2i();

                string animation = "";
                bool splitSprite = false;
                bool splitSpritePixel = false;
                int splitSpriteColumns = 0;
                int splitSpriteRows = 0;
                int columnPos = 0;
                int rowPos = 0;

                byte r = 0;
                byte g = 0;
                byte b = 0;

                int maxLayer = 0;

                string nameSceneStart = nameScene;
                
                foreach (XmlElement xnode in xRoot)
                {
                    if (initObjects) {
                        if (xnode.Name.Equals("title")) titleGame = xnode.InnerText;
                        else if (xnode.Name.Equals("widhtScreen")) widthScreen = uint.Parse(xnode.InnerText);
                        else if (xnode.Name.Equals("heightScreen")) heightScreen = uint.Parse(xnode.InnerText);
                        else if (xnode.Name.Equals("colorBackgroundR")) r = byte.Parse(xnode.InnerText);
                        else if (xnode.Name.Equals("colorBackgroundG")) g = byte.Parse(xnode.InnerText);
                        else if (xnode.Name.Equals("colorBackgroundB")) b = byte.Parse(xnode.InnerText);
                        else if (xnode.Name.Equals("fullScreen")) debug = xnode.InnerText.Equals("true");
                        else if (xnode.Name.Equals("deviceConnect")) deviceConnect = xnode.InnerText.Equals("true");
                        else if (xnode.Name.Equals("maxLayer")) maxLayer = int.Parse(xnode.InnerText);
                        else if (xnode.Name.Equals("nameSceneStart")) nameSceneStart = xnode.InnerText.Equals("") ? null : xnode.InnerText;
                        else if (xnode.Name.Equals("sizeMouseTexture")) sizeMouseTexture = float.Parse(xnode.InnerText, CultureInfo.InvariantCulture);
                        else if (xnode.Name.Equals("mouseInGame")) mouseInGame = xnode.InnerText.Equals("true");
                    }
                    if (xnode.Name.Equals("scene") && sceneLoad)
                    {
                        XmlNode xScene = xnode.Attributes.GetNamedItem("name");
                        if (!xScene.Value.Equals(nameSceneStart)) continue;
                        
                        List<GameObject> listObjects = new List<GameObject>();

                        foreach (XmlNode childnodeScene in xnode.ChildNodes)
                        {
                            if (childnodeScene.Name.Equals("gameObject"))
                            {
                                XmlNode attrName = childnodeScene.Attributes.GetNamedItem("name");
                                XmlNode attrLayer = childnodeScene.Attributes.GetNamedItem("layer");

                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("x"))
                                    {
                                        x = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("y"))
                                    {
                                        y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("tag"))
                                    {
                                        tag = childnode.InnerText;
                                    }
                                    if (childnode.Name.Equals("sizeX"))
                                    {
                                        sizeX = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("sizeY"))
                                    {
                                        sizeY = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("rotate"))
                                    {
                                        rotate = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("nameTexture"))
                                    {
                                        nameTexture = childnode.InnerText + ".bubla";
                                    }
                                    else if (childnode.Name.Equals("point"))
                                    {
                                        point = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("resourse"))
                                    {
                                        resourse = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("smooth"))
                                    {
                                        smooth = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("repited"))
                                    {
                                        repited = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("collider"))
                                    {
                                        collider = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("trigger"))
                                    {
                                        trigger = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("fixedRotation"))
                                    {
                                        fixedRotation = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("fixedRotationTexture"))
                                    {
                                        fixedRotationTexture = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("angle"))
                                    {
                                        angle = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("typeCollider"))
                                    {
                                        typeCollider = childnode.InnerText;
                                    }
                                    else if (childnode.Name.Equals("animation"))
                                    {
                                        animation = childnode.InnerText;
                                    }
                                    else if (childnode.Name.Equals("mass"))
                                    {
                                        mass = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    else if (childnode.Name.Equals("sizeAddColliderX"))
                                    {
                                        sizeAddCollider.X = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("sizeAddColliderY"))
                                    {
                                        sizeAddCollider.Y = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("posAddColliderX"))
                                    {
                                        posAddCollider.X = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("posAddColliderY"))
                                    {
                                        posAddCollider.Y = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("splitSprite"))
                                    {
                                        splitSprite = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("splitSpritePixel"))
                                    {
                                        splitSpritePixel = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("splitSpriteColumns"))
                                    {
                                        splitSpriteColumns = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("splitSpriteRows"))
                                    {
                                        splitSpriteRows = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("splitSpriteColumnPos"))
                                    {
                                        columnPos = int.Parse(childnode.InnerText);
                                    }
                                    else if (childnode.Name.Equals("splitSpriteRowPos"))
                                    {
                                        rowPos = int.Parse(childnode.InnerText);
                                    }
                                }
                                try
                                {
                                    listObjects.Add(
                                    new GameObject(attrName.Value, tag, x, y, new Vector2f(sizeX, sizeY),
                                        new Vector2i(splitSpriteRows, splitSpriteColumns), animation, new Vector2i(rowPos, columnPos), 
                                        nameTexture, point, resourse, collider, splitSprite, splitSpritePixel, trigger,
                                        mass, sizeAddCollider, posAddCollider, int.Parse(attrLayer.Value), fixedRotation, angle, rotate, smooth, repited,
                                        fixedRotationTexture, typeCollider));
                                } catch {
                                    listObjects.Add(
                                    new GameObject(attrName.Value, tag, x, y, new Vector2f(sizeX, sizeY), 
                                        new Vector2i(splitSpriteRows, splitSpriteColumns), animation, new Vector2i(rowPos, columnPos),
                                        nameTexture.Replace(".bubla", ".png"), point, resourse, collider, splitSprite, splitSpritePixel,
                                        trigger, mass, sizeAddCollider, posAddCollider, int.Parse(attrLayer.Value), fixedRotation, angle, rotate, smooth, repited,
                                        fixedRotationTexture, typeCollider));
                                };
                            }
                            else if (childnodeScene.Name.Equals("sound"))
                            {
                                XmlNode attr = childnodeScene.Attributes.GetNamedItem("name");

                                string path = "";
                                bool loop = false;
                                float volume = 0;
                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("path"))
                                    {
                                        path = childnode.InnerText + ".bubla";
                                    }
                                    else if (childnode.Name.Equals("loop"))
                                    {
                                        loop = childnode.InnerText.Equals("true");
                                    }
                                    else if (childnode.Name.Equals("volume"))
                                    {
                                        volume = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                }
                                    Sound.GetSounds().Add(attr.Value, new Sound(path, loop, volume));
                            }
                            else if (childnodeScene.Name.Equals("text"))
                            {
                                XmlNode attr = childnodeScene.Attributes.GetNamedItem("name");

                                Vector2f pos = new Vector2f();
                                uint size = 0;
                                string text = "";
                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("x"))
                                    {
                                        pos.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("y"))
                                    {
                                        pos.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("size"))
                                    {
                                        size = uint.Parse(childnode.InnerText);
                                    }
                                    if (childnode.Name.Equals("text"))
                                    {
                                        text = childnode.InnerText;
                                    }
                                }
                                new Text(attr.Value, pos, size, text);
                            }
                            else if (childnodeScene.Name.Equals("image"))
                            {
                                XmlNode attr = childnodeScene.Attributes.GetNamedItem("name");

                                string path = "";
                                Vector2f pos = new Vector2f();
                                Vector2f scale = new Vector2f();
                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("nameTexture"))
                                    {
                                        path = childnode.InnerText + ".bubla";
                                    }
                                    if (childnode.Name.Equals("x"))
                                    {
                                        pos.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("y"))
                                    {
                                        pos.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeX"))
                                    {
                                        scale.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeY"))
                                    {
                                        scale.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                }
                                try
                                {
                                    new Image(attr.Value, path, pos, scale);
                                }
                                catch { new Image(attr.Value, path.Replace(".bubla", ".png"), pos, scale); };
                            }
                            else if (childnodeScene.Name.Equals("effect"))
                            {
                                XmlNode attr = childnodeScene.Attributes.GetNamedItem("name");

                                string path = "";
                                Vector2f pos = new Vector2f();
                                float size = 0;
                                int count = 0;
                                TypeEffect typeEffect = TypeEffect.PULSE;
                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("nameTexture"))
                                    {
                                        path = childnode.InnerText + ".bubla";
                                    }
                                    if (childnode.Name.Equals("x"))
                                    {
                                        pos.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("y"))
                                    {
                                        pos.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeX"))
                                    {
                                        size = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("countParticle"))
                                    {
                                        count = int.Parse(childnode.InnerText);
                                    }
                                    if (childnode.Name.Equals("typeEffect"))
                                    {
                                        switch (childnode.InnerText)
                                        {
                                            case "pulse":
                                                typeEffect = TypeEffect.PULSE;
                                                break;
                                        }
                                    }
                                }
                                try
                                {
                                    new Effect(attr.Value, path, pos, size, count, typeEffect, false, -1, null);
                                }
                                catch { new Effect(attr.Value, path.Replace(".bubla", ".png"), pos, size, count, typeEffect, false, -1, null); };
                            }
                            else if (childnodeScene.Name.Equals("bossbar"))
                            {
                                XmlNode attr = childnodeScene.Attributes.GetNamedItem("name");

                                Vector2f pos = new Vector2f();
                                Vector2f size = new Vector2f();
                                Rgb rgbBackground = new Rgb();
                                Rgb rgbInside = new Rgb();
                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("x"))
                                    {
                                        pos.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("y"))
                                    {
                                        pos.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeX"))
                                    {
                                        size.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeY"))
                                    {
                                        size.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("rgbRInside"))
                                    {
                                        rgbInside.R = byte.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("rgbGInside"))
                                    {
                                        rgbInside.G = byte.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("rgbBInside"))
                                    {
                                        rgbInside.B = byte.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("rgbROutside"))
                                    {
                                        rgbBackground.R = byte.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("rgbGOutside"))
                                    {
                                        rgbBackground.G = byte.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("rgbBOutside"))
                                    {
                                        rgbBackground.B = byte.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                }
                                new Bossbar(attr.Value, pos, size, rgbBackground, rgbInside);
                            }
                            else if (childnodeScene.Name.Equals("button"))
                            {
                                XmlNode attr = childnodeScene.Attributes.GetNamedItem("name");

                                string path = "";
                                Vector2f pos = new Vector2f();
                                Vector2f scale = new Vector2f();
                                foreach (XmlNode childnode in childnodeScene.ChildNodes)
                                {
                                    if (childnode.Name.Equals("nameTexture"))
                                    {
                                        path = childnode.InnerText + ".bubla";
                                    }
                                    if (childnode.Name.Equals("x"))
                                    {
                                        pos.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("y"))
                                    {
                                        pos.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeX"))
                                    {
                                        scale.X = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                    if (childnode.Name.Equals("sizeY"))
                                    {
                                        scale.Y = float.Parse(childnode.InnerText, CultureInfo.InvariantCulture);
                                    }
                                }
                                try
                                {
                                    Button.GetButtons().Add(new Button(attr.Value, path, pos, scale));
                                }
                                catch { Button.GetButtons().Add(new Button(attr.Value, path.Replace(".bubla", ".png"), pos, scale)); };
                            }
                        }
                        
                        // add all gameObjects
                        int layer = 0;
                        while(true)
                        {
                            foreach(GameObject gameObject in listObjects)
                            {
                                if (gameObject.Layer == layer)
                                {
                                    GameObject.GetGameObjects().Add(gameObject.Name, gameObject);
                                }
                            }
                            if (layer == maxLayer) break;
                            layer++;
                        }
                        Console.WriteLine($"загрузка обьектов прекратилась на слое: {layer}");
                        sceneLoad = false;
                        break;
                    }
                }
                LuaScript.Create(nameSceneStart);
                if (nameSceneStart == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("!ОШИБКА!\t вы указали не существующию сцену! зайдите свойства и исправте параметр scene start");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (initObjects) colorBackgroundRGB = new byte[]{r, g, b};
            }
        }

        private static Vector2f operationCamera = new Vector2f(0, 0);
        private static Vector2f operationCameraUI = new Vector2f(0, 0);

        public void CameraAddPosition(float x, float y)
        {
            operationCamera.X = x;
            operationCamera.Y = y;
            camera.Move(operationCamera);
        }

        public void CameraSetPosition(float x, float y)
        {
            operationCamera.X = x;
            operationCamera.Y = y;
            camera.Center = operationCamera;
        }

        public void CameraSetPositionSlow(float x, float y, float speed)
        {
            float distance = (float)Math.Sqrt(Math.Pow(x - camera.Center.X, 2) + Math.Pow(y - camera.Center.Y, 2));
            if (distance < speed) return;
            float tempX = (x - camera.Center.X) / distance * speed;
            float tempY = (y - camera.Center.Y) / distance * speed;
            
            operationCamera.X += tempX;
            operationCamera.Y += tempY;
            camera.Center = operationCamera;
        }

        public void CameraUISetPosition(float x, float y)
        {
            operationCameraUI.X = x;
            operationCameraUI.Y = y;
            cameraUI.Center = operationCameraUI;
        }

        public void CameraUISetPositionSlow(float x, float y, float speed)
        {
            float distance = (float)Math.Sqrt(Math.Pow(x - cameraUI.Center.X, 2) + Math.Pow(y - cameraUI.Center.Y, 2));
            if (distance < speed) return;
            float tempX = (x - cameraUI.Center.X) / distance * speed;
            float tempY = (y - cameraUI.Center.Y) / distance * speed;
            
            operationCameraUI.X = tempX;
            operationCameraUI.Y = tempY;
            cameraUI.Center = operationCameraUI;
        }

        public void CameraZoom(float zoom)
        {
            camera.Zoom(zoom);
        }

        public void CameraUIZoom(float zoom)
        {
            cameraUI.Zoom(zoom);
        }

        private static bool sceneNext = false;
        private static string nameSceneNext = "";

        public static void SSetScene(string name)
        {
            if (!initObjects)
            {
                nameSceneNext = name;
                return;
            }
            if (sceneNext) return;
            sceneNext = true;
            addAllGameObjects(name);
            World.Init();
            Save.Init();
            OfflineTracker.Init();

            width = !debug ? SFML.Window.VideoMode.DesktopMode.Width : defailtWidthScreen;
            height = !debug ? SFML.Window.VideoMode.DesktopMode.Height : defailtHeightScreen;

            camera = new View(new FloatRect(0, 0, width * (debug ? 2 : 1), height * (debug ? 2 : 1)));
            cameraUI = new View(new FloatRect(0, 0, width * (debug ? 2 : 1), height * (debug ? 2 : 1)));

            LuaScript.InvokeMethod("Start");

            sceneNext = false;
        }

        public void SetScene(string name)
        {
            SSetScene(name);
        }

        public static uint WidthScreen
        {
            get { return widthScreen; }
        }

        public static uint HeightScreen
        {
            get { return heightScreen; }
        }

        public static uint DefaultWidthScreen
        {
            get { return defailtWidthScreen; }
        }

        public static uint DefaultHeightScreen
        {
            get { return defailtHeightScreen; }
        }

        public static bool Debug
        {
            get { return debug; }
        }

        public static Random Random
        {
            get { return random; }
        }

        public static RenderWindow GetWindow()
        {
            return window;
        }

        public static Vector2f GetMousePosition()
        {
            return new Vector2f(SFML.Window.Mouse.GetPosition().X - window.Position.X, SFML.Window.Mouse.GetPosition().Y - window.Position.Y);
        }

        public static float SDelta()
        {
            return delta;
        }

        public float Delta()
        {
            return delta;
        }

        public float GetFps()
        {
            return fps;
        }

        public void SetFps(int fps)
        {
            if (windowCreate) window.SetFramerateLimit((uint)fps);
        }

        public static float DeltaTime
        {
            get { return delta < 1 ? 1 : delta; }
        }

        public static void Close()
        {
            try
            {
                window.Close();
            } catch (NullReferenceException e) { }
        }

        public static void EventConnectDevice()
        {
            LuaScript.InvokeMethod("OnDeviceConnect");
        }

        public static void EventDisconnectDevice()
        {
            LuaScript.InvokeMethod("OnDeviceDisconect");
        }

        private static Timer readDevice = new Timer(0.0000005f);

        private static float Distance(Vector2f pos1, Vector2f pos2)
        {
            return (float)Math.Sqrt(Math.Pow((pos2.X - pos1.X), 2) + Math.Pow((pos2.Y - pos1.Y), 2));
        }

        public static void EventReadDevice(string readLine)
        {
            readDevice.Add(-delta);
            if (readDevice.GetFloat() > 0) return;
            readDevice.Reset();
            string[] readLines = readLine.Split(' ');
            int[] keys = new int[readLines.Length];
            for(int i = 0; i < keys.Length ;i++)
            {
                keys[i] = int.Parse(readLines[i]);
            }

            LuaScript.InvokeMethod("OnDeviceRead", new Object[] { keys });
        }
            
        public float GetDesktopHeight()
        {
            return SFML.Window.VideoMode.DesktopMode.Height;
        }

        public float GetDesktopWidht()
        {
            return SFML.Window.VideoMode.DesktopMode.Width;
        }

        public float GetViewHeight()
        {
            return defailtHeightScreen;
        }

        public float GetViewWidht()
        {
            return defailtWidthScreen;
        }

        public static float SCursorX()
        {
            return GetMousePosition().X / (debug ? 0.515f : 1);
        }

        public static float SCursorY()
        {
            return GetMousePosition().Y / (debug ? 0.57f : 1);
        }

        public float CursorX()
        {
            return GetMousePosition().X / (debug ? 0.515f : 1);
        }

        public float CursorY()
        {
            return GetMousePosition().Y / (debug ? 0.57f : 1);
        }

        public static Box2DX.Dynamics.World GetWorld()
        {
            return world;
        }

        public void SetGravity(float x, float y)
        {
            world.Gravity = new Vec2(x, y);
        }

        public static Vec2 GetGravity()
        {
            return world.Gravity;
        }

        public static void SetColorGameObjects(int r, int g, int b)
        {
            foreach (KeyValuePair<string, GameObject> listGameObject in GameObject.GetGameObjects())
            {
                GameObject gameObject = listGameObject.Value;
                gameObject.SetColor(r, g, b);
            }
        }

        public static void AnimationAchivcment(string name, string headText, string lore)
        {
            string text = "";
            backgroundAcivment.Position = new Vector2f(cameraUI.Size.X - backgroundAcivment.Size.X + backgroundAcivment.Size.X / 2, cameraUI.Size.Y + backgroundAcivment.Size.Y / 2);

            int j = 0;
            int k = 0;
            foreach (char _char in lore.ToCharArray())
            {
                j++;
                if (k == 1 && j == 18) break;
                else if (j == 21)
                {
                    text += "\n";
                    j = 0;
                    k = 1;
                }
                text += _char;
            }
            text += "...";
            loreAchivment.DisplayedString = text;
            loreAchivment.Position = new Vector2f(cameraUI.Size.X - backgroundAcivment.Size.X + backgroundAcivment.Size.X / 2 - 80, cameraUI.Size.Y + backgroundAcivment.Size.Y / 2);

            text = "";

            int i = 0;
            foreach (char _char in headText.ToCharArray())
            {
                if (i++ == 12) break;
                text += _char;
            }
            text += "...";
            headTextAchivment.DisplayedString = text;
            headTextAchivment.Position = new Vector2f(cameraUI.Size.X - backgroundAcivment.Size.X + backgroundAcivment.Size.X / 2 - 80, cameraUI.Size.Y + backgroundAcivment.Size.Y / 2 - 50);
            
            animAcivmentNow = true;
            nameObjectAchivment = name;
            timerAcivment.Reset();
            Achievement.PlaySound();
        }

        public static bool AnimAcivmentNow { 
            get => animAcivmentNow; 
        }

        public static View GetCameraUI()
        {
            return cameraUI;
        }

        public static string GetTitle()
        {
            return titleGame;
        }

        private static void CustomError()
        {
            int[] array = { };
            array[1] = 1;
        }
    }
}
