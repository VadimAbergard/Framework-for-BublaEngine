using Bubla;
using System;
using System.Collections.Generic;

namespace Bubla
{
    public class Value
    {
        private static Dictionary<string, Value> values = new Dictionary<string, Value>();
        private int intValue;
        private float floatValue;
        private string stringValue;

        public void Reg(string name)
        {
            values.Add(name, this);
        }

        public static int GetInt(string name)
        {
            try
            {
                return values[name].intValue;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return 0;
        }

        public static float GetFloat(string name)
        {
            try
            {
                return values[name].floatValue;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return 0;
        }

        public static string GetString(string name)
        {
            try
            {
                return values[name].stringValue;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }

        public static object Add(string name, int value)
        {
            try
            {
                values[name].intValue += value;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }

        public static object Add(string name, float value)
        {
            try
            {
                values[name].floatValue += value;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }

        public static object Add(string name, string value)
        {
            try
            {
                values[name].stringValue += value;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }

        public static object Set(string name, int value)
        {
            try
            {
                values[name].intValue = value;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }

        public static object Set(string name, float value)
        {
            try
            {
                values[name].floatValue = value;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }

        public static object Set(string name, string value)
        {
            try
            {
                values[name].stringValue = value;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Упс! \"value\": \"" + name + "\" не было ни когда зарегестрировано!");
                Game.Close();
            }
            return null;
        }



    }
}
