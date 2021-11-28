using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

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
