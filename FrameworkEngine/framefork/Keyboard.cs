
namespace Bubla
{
    public class Keyboard
    {
        public bool IsKeyPressed(string key)
        {
            // this digroid code
            return Equals(SFML.Window.Keyboard.Key.Q, key) || Equals(SFML.Window.Keyboard.Key.W, key) ||
                Equals(SFML.Window.Keyboard.Key.E, key) || Equals(SFML.Window.Keyboard.Key.R, key) ||
                Equals(SFML.Window.Keyboard.Key.T, key) || Equals(SFML.Window.Keyboard.Key.Y, key) || 
                Equals(SFML.Window.Keyboard.Key.U, key) || Equals(SFML.Window.Keyboard.Key.I, key) || 
                Equals(SFML.Window.Keyboard.Key.O, key) || Equals(SFML.Window.Keyboard.Key.P, key) ||
                Equals(SFML.Window.Keyboard.Key.A, key) || Equals(SFML.Window.Keyboard.Key.S, key) ||
                Equals(SFML.Window.Keyboard.Key.D, key) || Equals(SFML.Window.Keyboard.Key.F, key) || 
                Equals(SFML.Window.Keyboard.Key.G, key) || Equals(SFML.Window.Keyboard.Key.H, key) ||
                Equals(SFML.Window.Keyboard.Key.J, key) || Equals(SFML.Window.Keyboard.Key.K, key)||
                Equals(SFML.Window.Keyboard.Key.L, key) || Equals(SFML.Window.Keyboard.Key.Z, key) ||
                Equals(SFML.Window.Keyboard.Key.X, key) || Equals(SFML.Window.Keyboard.Key.C, key) || 
                Equals(SFML.Window.Keyboard.Key.V, key) || Equals(SFML.Window.Keyboard.Key.B, key) || 
                Equals(SFML.Window.Keyboard.Key.N, key) || Equals(SFML.Window.Keyboard.Key.M, key) ||
                Equals(SFML.Window.Keyboard.Key.Space, key) || Equals(SFML.Window.Keyboard.Key.Num1, key) ||
                Equals(SFML.Window.Keyboard.Key.Num2, key) || Equals(SFML.Window.Keyboard.Key.Num3, key) || 
                Equals(SFML.Window.Keyboard.Key.Num4, key) || Equals(SFML.Window.Keyboard.Key.Num5, key) ||
                Equals(SFML.Window.Keyboard.Key.Num6, key) || Equals(SFML.Window.Keyboard.Key.Num7, key) ||
                Equals(SFML.Window.Keyboard.Key.Num8, key) || Equals(SFML.Window.Keyboard.Key.Num9, key) || 
                Equals(SFML.Window.Keyboard.Key.Num0, key) || Equals(SFML.Window.Keyboard.Key.F1, key) || 
                Equals(SFML.Window.Keyboard.Key.F2, key) || Equals(SFML.Window.Keyboard.Key.F3, key) || 
                Equals(SFML.Window.Keyboard.Key.F4, key) || Equals(SFML.Window.Keyboard.Key.F5, key) || 
                Equals(SFML.Window.Keyboard.Key.F6, key) || Equals(SFML.Window.Keyboard.Key.F7, key) || 
                Equals(SFML.Window.Keyboard.Key.F8, key) || Equals(SFML.Window.Keyboard.Key.F9, key) || 
                Equals(SFML.Window.Keyboard.Key.F10, key) || Equals(SFML.Window.Keyboard.Key.F11, key) || 
                Equals(SFML.Window.Keyboard.Key.F12, key) || Equals(SFML.Window.Keyboard.Key.Tab, key) || 
                Equals(SFML.Window.Keyboard.Key.RShift, key) || Equals(SFML.Window.Keyboard.Key.LShift, key) || 
                Equals(SFML.Window.Keyboard.Key.RControl, key) || Equals(SFML.Window.Keyboard.Key.LControl, key) || 
                Equals(SFML.Window.Keyboard.Key.RAlt, key) || Equals(SFML.Window.Keyboard.Key.LAlt, key) || 
                Equals(SFML.Window.Keyboard.Key.Return, key) || Equals(SFML.Window.Keyboard.Key.Escape, key);
        }

        private static bool Equals(SFML.Window.Keyboard.Key key, string keyName)
        {
            return SFML.Window.Keyboard.IsKeyPressed(key) && keyName.ToLower().Equals(key.ToString().ToLower());
        }
    }
}
