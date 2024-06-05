
namespace Bubla
{
    public class Mouse
    {
        public bool IsKeyPressed(int key)
        {
            switch (key)
            {
                case 0:
                    return SFML.Window.Mouse.IsButtonPressed(SFML.Window.Mouse.Button.Left);
                case 1:
                    return SFML.Window.Mouse.IsButtonPressed(SFML.Window.Mouse.Button.Middle);
                case 2:
                    return SFML.Window.Mouse.IsButtonPressed(SFML.Window.Mouse.Button.Right);
                default: 
                    return false;
            }
        }
    }
}
