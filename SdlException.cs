namespace langtons_ant_1
{
    using SDL2;
    using System;
    public class SdlException : Exception
    {
        public SdlException()
            : base()
        { }

        public SdlException(string method)
            : base($"{method}: {SDL.SDL_GetError()}")
        { }
    }
}