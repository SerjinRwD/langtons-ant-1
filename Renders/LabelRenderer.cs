namespace langtons_ant_1.Renders
{
    using langtons_ant_1.Entities;
    using langtons_ant_1.Helpers;
    using SDL2;
    using System;

    public class LabelRenderer : AbstractRenderer<Label>
    {
        protected IntPtr _font;
        protected SDL.SDL_Color _color;

        public LabelRenderer(IntPtr renderer, IntPtr font, SDL.SDL_Color color, Label target)
            : base(renderer, target)
        {
            _font = font;
            _color = color;
            _target.LabelChanged += OnLabelChanged;

            Draw();
        }

        public void Draw()
        {
            var oldSurface = _surface;
            var oldTexture = _texture;

            _surface = SDL_ttf.TTF_RenderUTF8_Solid(_font, _target.Text, _color);
            
            if(_surface == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL_ttf.TTF_RenderUTF8_Solid));
            }
            
            _texture = SDL.SDL_CreateTextureFromSurface(_renderer, _surface);

            if(_texture == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL.SDL_CreateTextureFromSurface));
            }

            SDL.SDL_DestroyTexture(oldTexture);
            SDL.SDL_FreeSurface(oldSurface);
        }

        private void OnLabelChanged(object sender, EventArgs args)
        {
            Draw();
        }

        public override void Render()
        {
            SdlDrawing.RenderTexture(_texture, _renderer, _target.X, _target.Y);
        }
    }
}