namespace langtons_ant_1.Renders
{
    using SDL2;
    using System;
    public abstract class AbstractRenderer<T> : IRenderer<T>, IDisposable
    {
        protected T _target;
        protected IntPtr _renderer;
        protected IntPtr _surface;
        protected IntPtr _texture;

        public abstract void Render();

        public AbstractRenderer(IntPtr renderer, T target)
        {
            _renderer = renderer;
            _target = target;
        }

        public void Dispose(){  
            Dispose(true);  
            GC.SuppressFinalize(this);  
        } 

        protected virtual void Dispose(bool disposing){  
            if (disposing){  
                if (_surface != IntPtr.Zero)
                {
                    SDL.SDL_FreeSurface(_surface);
                }
                if(_texture != IntPtr.Zero)
                {
                    SDL.SDL_DestroyTexture(_texture);
                }
            }  
        }
    }
}