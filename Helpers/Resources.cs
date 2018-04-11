namespace langtons_ant_1.Helpers
{
    using SDL2;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public static class Resources
    {
        private static readonly Regex HeximalColorRGBACodeMask = new Regex(@"^#[0-9A-Fa-f]{6,8}$");
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

        public static SDL.SDL_Color ParseColorCode(string rgbaCode)
        {
            if(!HeximalColorRGBACodeMask.IsMatch(rgbaCode))
            {
                throw new ArgumentException(
                    @"Некорректный код RGB цвета. Ожидалась строка вида #ffffffff.",
                    nameof(rgbaCode));
            }

            var rStr = rgbaCode.Substring(1, 2);
            var gStr = rgbaCode.Substring(3, 2);
            var bStr = rgbaCode.Substring(5, 2);
            var aStr = rgbaCode.Substring(7, 2);

            var c = new SDL.SDL_Color();

            c.r = byte.Parse(rStr, NumberStyles.AllowHexSpecifier);
            c.g = byte.Parse(gStr, NumberStyles.AllowHexSpecifier);
            c.b = byte.Parse(bStr, NumberStyles.AllowHexSpecifier);
            c.a = byte.Parse(aStr, NumberStyles.AllowHexSpecifier);

            return c;
        }
    }
}