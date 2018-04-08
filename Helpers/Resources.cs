namespace langtons_ant_1.Helpers
{
    using SDL2;
    using System;
    using System.IO;
    using System.Reflection;

    public static class Resources
    {
        private static string _basePath;

        public static string BasePath
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_basePath))
                {
                    _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }

                return _basePath;
            }
        }

        public static string GetFilePath(string fileName)
        {
            var path = Path.Combine(BasePath, "resources", fileName);

            if(!File.Exists(path))
            {
                throw new FileNotFoundException(fileName);
            }

            return path;
        }

        public static IntPtr LoadTextureFromBitmap(string path, IntPtr renderer)
        {
            var texture = IntPtr.Zero;

            var image = SDL.SDL_LoadBMP(path);

            if(image != IntPtr.Zero)
            {
                texture = SDL.SDL_CreateTextureFromSurface(renderer, image);
                SDL.SDL_FreeSurface(image);

                if(texture == IntPtr.Zero)
                {
                    SdlLogger.Error(nameof(SDL.SDL_CreateTextureFromSurface));   
                }
            }
            else
            {
                SdlLogger.Error(nameof(SDL.SDL_LoadBMP));
            }

            return texture;
        }

        public static IntPtr LoadTextureFromImage(string path, IntPtr renderer)
        {
            var texture = SDL_image.IMG_LoadTexture(renderer, path);

            if(texture == IntPtr.Zero)
            {
                SdlLogger.Error(nameof(SDL_image.IMG_LoadTexture));
            }

            return texture;
        }
    }
}