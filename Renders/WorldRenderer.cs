namespace langtons_ant_1.Renders
{
    using langtons_ant_1.Entities;
    using SDL2;
    using System;

    public class WorldRenderer : AbstractRenderer<World>
    {
        SDL.SDL_Rect _zoomRect;
        SDL.SDL_Rect _frameRect;
        double _aspectRatio;

        public WorldRenderer(IntPtr renderer, World world)
            : base(renderer, world)
        {
            _surface = SDL.SDL_CreateRGBSurfaceWithFormat(0, _target.W, _target.H, 32, SDL.SDL_PIXELFORMAT_RGBA8888);

            var c = _target.Colors[0];

            uint fillColor;

            unsafe
            {
                var fmt = (*((SDL.SDL_Surface*)_surface.ToPointer())).format;
                fillColor = SDL.SDL_MapRGBA(fmt, c.r, c.g, c.b, c.a);
            }

            var dst = new SDL.SDL_Rect();

            for(var x = 0; x < _target.W; x++)
            {
                for(var y = 0; y < _target.H; y++)
                {
                    dst.x = x;
                    dst.y = y;
                    dst.w = 1;
                    dst.h = 1;
                    SDL.SDL_FillRect(_surface, ref dst, (uint)fillColor);
                }
            }

            _texture = SDL.SDL_CreateTextureFromSurface(_renderer, _surface);

            _target.WorldCellChanged += OnWorldCellChanged;

            _frameRect = new SDL.SDL_Rect();
            _frameRect.x = 0;
            _frameRect.y = 0;
            _frameRect.w = 640;
            _frameRect.h = 480;

            _aspectRatio = (double)_frameRect.w / (double)_frameRect.h;

            _zoomRect = new SDL.SDL_Rect();
            _zoomRect.x = 0;
            _zoomRect.y = 0;
            _zoomRect.w = _frameRect.w;
            _zoomRect.h = _frameRect.h;

        }

        public void AddZoom(int value)
        {
            _zoomRect.w += value;
            _zoomRect.h = (int)((double)_zoomRect.w / _aspectRatio);
        }

        public void MoveZoom(int dx, int dy)
        {
            var newx = _zoomRect.x + dx;
            var newy = _zoomRect.y + dy;

            if(newx < 0)
            {
                newx = 0;
            }

            if((newx + _zoomRect.w) > _target.W)
            {
                newx = _zoomRect.x;
            }

            if(newy < 0)
            {
                newy = 0;
            }

            if((newy + _zoomRect.h) > _target.H)
            {
                newy = _zoomRect.y;
            }

            _zoomRect.x = newx;
            _zoomRect.y = newy;
        }

        public void ResetZoom()
        {
            _zoomRect.x = 0;
            _zoomRect.y = 0;
            _zoomRect.w = _frameRect.w ;
            _zoomRect.h = _frameRect.h;
        }

        public override void Render()
        {
            SDL.SDL_RenderCopy(_renderer, _texture, ref _zoomRect, ref _frameRect);// IntPtr.Zero, IntPtr.Zero);
        }

        private void OnWorldCellChanged(object sender, WorldCellChangedEventArgs args)
        {
            var oldTexture = _texture;

            var c = _target.Colors[args.NewColorId];
            uint fillColor;

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