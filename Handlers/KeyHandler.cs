namespace langtons_ant_1.Handlers
{
    using SDL2;
    using System;
    using System.Collections.Generic;

    public class KeyHandler : Dictionary<SDL.SDL_Keycode, Action>
    {
        public void Handle(SDL.SDL_Keycode keycode)
        {
            if(!ContainsKey(keycode))
            {
                return;
            }

            this[keycode]?.Invoke();
        }
    }
}