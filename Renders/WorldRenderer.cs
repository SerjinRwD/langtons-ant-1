namespace langtons_ant_1.Renders
{
    using langtons_ant_1.Entities;
    using SDL2;
    using System;

    public class WorldRenderer
    {
        private World _world;
        private IntPtr _renderer;
        private IntPtr _surface;
        private IntPtr _texture;

        public WorldRenderer(IntPtr renderer, World world)
        {
            _renderer = renderer;
            _world = world;
            _surface = SDL.SDL_CreateRGBSurfaceWithFormat(0, _world.W, _world.H, 32, SDL.SDL_PIXELFORMAT_RGBA8888);

            var c = _world.Colors[0];

            uint fillColor; // = ((uint)c.r << 6) | ((uint)c.g << 4) | ((uint)c.b << 2); // | c.a;

            unsafe
            {
                var fmt = (*((SDL.SDL_Surface*)_surface.ToPointer())).format;
                fillColor = SDL.SDL_MapRGBA(fmt, c.r, c.g, c.b, c.a);
            }

            var dst = new SDL.SDL_Rect();

            for(var x = 0; x < _world.W; x++)
            {
                for(var y = 0; y < _world.H; y++)
                {
                    dst.x = x;
                    dst.y = y;
                    dst.w = 1;
                    dst.h = 1;
                    SDL.SDL_FillRect(_surface, ref dst, (uint)fillColor);
                }
            }

            _texture = SDL.SDL_CreateTextureFromSurface(_renderer, _surface);

            _world.WorldCellChanged += OnWorldCellChanged;
        }

        public void Render()
        {
            SDL.SDL_RenderCopy(_renderer, _texture, IntPtr.Zero, IntPtr.Zero); // ref fullsize
        }

        private void OnWorldCellChanged(object sender, WorldCellChangedEventArgs args)
        {
            var oldTexture = _texture;

            var c = _world.Colors[args.NewColorId];
            uint fillColor; // = ((uint)c.r << 6) | ((uint)c.g << 4) | ((uint)c.b << 2); // | c.a;

            unsafe
            {
                var fmt = (*((SDL.SDL_Surface*)_surface.ToPointer())).format;
                fillColor = SDL.SDL_MapRGBA(fmt, c.r, c.g, c.b, c.a);
            }

            var dst = new SDL.SDL_Rect();

            dst.x = args.X;
            dst.y = args.Y;
            dst.w = 1;
            dst.h = 1;
            SDL.SDL_FillRect(_surface, ref dst, (uint)fillColor);

            _texture = SDL.SDL_CreateTextureFromSurface(_renderer, _surface);

            SDL.SDL_DestroyTexture(oldTexture);
        }
    }
}