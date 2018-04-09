namespace langtons_ant_1.Renders
{
    using langtons_ant_1.Entities;
    using SDL2;
    using System;

    public class WorldRenderer
    {
        private World _world;
        private IntPtr _renderer;

        public WorldRenderer(IntPtr renderer, World world)
        {
            _renderer = renderer;

            _world = world;
            _world.WorldCellChanged += OnWorldCellChanged;

            var c = _world.Colors[0];

            SDL.SDL_SetRenderDrawColor(_renderer, c.r, c.g, c.b, 255);

            for(var x = 0; x < _world.W; x++)
            {
                for(var y = 0; y < _world.H; y++)
                {
                    SDL.SDL_RenderDrawPoint(_renderer, x, y);
                }
            }
        }

        private void OnWorldCellChanged(object sender, WorldCellChangedEventArgs args)
        {
            var c = _world.Colors[args.NewColorId];

            SDL.SDL_SetRenderDrawColor(_renderer, c.r, c.g, c.b, 255);
            SDL.SDL_RenderDrawPoint(_renderer, args.X, args.Y);
        }
    }
}