namespace langtons_ant_1.Helpers
{
    using SDL2;
    using System;
    
    public static class SdlLogger
    {
        /// <summary>
        /// Фиксируем критическую ошибку. Если handler возвращает false, бросаем исключение
        /// </summary>
        /// <param name="source">Источник ошибки</param>
        /// <param name="handler">Делегат обработки ошибки</param>
        public static void Fatal(string source, Func<bool> handler = null)
        {
            var msg = $"{source} Error: {SDL.SDL_GetError()}";

            // Если обработчик есть и вернул true, то не бросаем исключение, пишем в Console
            if(!(handler != null && handler()))
            {
                throw new Exception(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        public static void Error(string source)
        {
            Console.WriteLine($"{source} Error: {SDL.SDL_GetError()}");
        }
}
}