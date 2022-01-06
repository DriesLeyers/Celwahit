using Microsoft.Xna.Framework.Input;

namespace Celwahit.InputReaders
{
    static class KeyboardReader
    {

        static public Keys[] GetKeys()
        {

            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

            return pressedKeys;
        }

    }
}
