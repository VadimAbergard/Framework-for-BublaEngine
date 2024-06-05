using Bubla;
using FrameworkEngine.framefork.world;
using LuaInterface;
using MyFramework.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lua = LuaInterface.Lua;

namespace FrameworkEngine.utils
{
    internal class LuaScript
    {
        //private static List<LuaScript> luaScripts = new List<LuaScript>();
        private static Lua lua = null;
        //private bool global;

        public static void Create(string nameScene) {
            if (!File.Exists($"assets\\mainScript{nameScene}.bubla")) return;
            StreamReader fsRead = new StreamReader(File.Open($"assets\\mainScript{nameScene}.bubla", FileMode.Open));

            string text = "";
            int data;
            while ((data = fsRead.Read()) != -1)
            {
                text += (char)data;
            }
            fsRead.Close();

            text = Encryption.DencrypText(text);

            lua = new Lua();

            lua["GameObject"] = new GameObject();
            lua["Game"] = new Game();

            lua["World"] = new World();
            lua["TiledMap"] = new TiledMap();
            lua["OfflineTracker"] = new OfflineTracker();
            lua["Save"] = new Save();
            lua["Sql"] = new Sql();
            lua["OtherFile"] = new OtherFile();

            lua["Button"] = new Button();
            lua["Bossbar"] = new Bossbar();
            lua["Effect"] = new Effect();
            lua["Image"] = new Image();
            lua["Text"] = new Text();
            lua["Sound"] = new Sound();

            lua["Command"] = new Command();
            lua["RobotSpeak"] = new RobotSpeak();
            lua["Keyboard"] = new Keyboard();
            lua["Mouse"] = new Mouse();
            lua["Timer"] = new Bubla.Timer(1);

            string addCode = @"
                function copyTable (originalTable)
                    local newTable = {};
                    for k,v in pairs(originalTable) do
                        newTable[k] = v;
                    end
                    return newTable;
                end
                
                ";

            Console.ForegroundColor = ConsoleColor.Yellow;
            lua.DoString(addCode + text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static object[] InvokeMethod(string nameMethod, object[] args = null)
        {
            if (lua == null) return null;
            object[] objects = null;

            Console.ForegroundColor = ConsoleColor.Yellow;
            LuaFunction func = lua[nameMethod] as LuaFunction;
            if (func != null)
            {
                if (args == null) objects = func.Call();
                else objects = func.Call(args);
                func.Dispose();
            }
            Console.ForegroundColor = ConsoleColor.White;

            return objects;
        }

        /*public object[] InvokeMethod(string nameMethod, object[] args = null)
        {
            object[] objects = null;

            Console.ForegroundColor = ConsoleColor.Yellow;
            LuaFunction func = lua[nameMethod] as LuaFunction;
            if (func != null)
            {
                if (args == null) objects = func.Call();
                else objects = func.Call(args);
                func.Dispose();
            }
            Console.ForegroundColor = ConsoleColor.White;

            return objects;
        }*/

        /*public static void InvokeMethodGlobal(string nameMethod, object[] args)
        {
            foreach (LuaScript luaScript in luaScripts) {
                if (!luaScript.global) continue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                LuaFunction func = luaScript.lua[nameMethod] as LuaFunction;
                if (func != null)
                {
                    if (args == null) func.Call();
                    else func.Call(args);
                    func.Dispose();
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }*/

        public static void Dispose()
        {
            if(lua != null) lua.Dispose();
        }

        /*public static List<LuaScript> GetLuaScripts()
        {
            return luaScripts;
        }*/
    }
}
