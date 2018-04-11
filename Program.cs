using langtons_ant_1.Entities;
using langtons_ant_1.Helpers;
using langtons_ant_1.Renders;
using SDL2;
using System;
using System.IO;

namespace langtons_ant_1
{
    class Program
    {
        private static IntPtr Renderer;
        private static IntPtr Window;
        private static World World;
        private static WorldRenderer WorldRenderer;

        static void Main(string[] args)
        {
            try
            {
                Init();

                var worldPath = Resources.GetFilePath("worlds/default.xml");
                World = new World(WorldMetadata.Load(worldPath));
                WorldRenderer = new WorldRenderer(Renderer, World);

                Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception were thrown. Message: {ex.Message}. Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                Quit();
            }
        }

        private static void Init()
        {
            if(SDL.SDL_Init(SDL.SDL_INIT_VIDEO) != 0)
            {
                throw new SdlException(nameof(SDL.SDL_Init));
            }

            Window = SDL.SDL_CreateWindow("Langton`s Ant", 0, 0, 640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if(Window == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL.SDL_CreateWindow));
            }

            Renderer = SDL.SDL_CreateRenderer(
                Window, -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if(Renderer == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL.SDL_CreateRenderer));
            }
        }

        private static void Run()
        {
            var quit = false;

            var e = new SDL.SDL_Event();

            while(!quit)
            {
                // event handling
                while(SDL.SDL_PollEvent(out e) > 0)
                {
                    if(e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        quit = true;
                    }

                    if(e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        switch(e.key.keysym.sym)
                        {
                            case SDL.SDL_Keycode.SDLK_ESCAPE:
                                quit = true;
                                break;
                        }
                    }
                }

                World.Update();

                // rendering
                SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
                SDL.SDL_RenderClear(Renderer);

                WorldRenderer.Render();

                SDL.SDL_RenderPresent(Renderer);
            }
        }

        private static void Quit()
        {
            if(!(Renderer == IntPtr.Zero))
            {
                SDL.SDL_DestroyRenderer(Renderer);
            }

            if(!(Window == IntPtr.Zero))
            {
                SDL.SDL_DestroyWindow(Window);
            }

            SDL.SDL_Quit();
        }

        private static void MakeLangtonTurmite()
        {
            var tst = new Entities.TransitionStateTable
            {
                Colors = 2,
                States = 1,
                Transitions = new Entities.Transition[]
                {
                    new Entities.Transition
                    {
                        OnStateId = 0,
                        OnColorId = 0,
                        Turn = Entities.TransitionTurn.Left,
                        NewColorId = 1,
                        NewStateId = 0,
                    },

                    new Entities.Transition
                    {
                        OnStateId = 0,
                        OnColorId = 1,
                        Turn = Entities.TransitionTurn.Right,
                        NewColorId = 0,
                        NewStateId = 0,
                    }
                }
            };

            TransitionStateTable.Save(Path.Combine(
                Resources.BasePath,
                "resources/turmites/langton.xml"), tst);
        }

        private static void MakeDefaultWorld()
        {
            var world = new WorldMetadata
            {
                W = 640,
                H = 480,

                Colors = new ColorMetadata[]
                {
                    new ColorMetadata { Id = 0, RGBACode = "#000000ff" },
                    new ColorMetadata { Id = 1, RGBACode = "#ffffffff" },
                    new ColorMetadata { Id = 2, RGBACode = "#ff0000ff" },
                    new ColorMetadata { Id = 3, RGBACode = "#00ff00ff" },
                    new ColorMetadata { Id = 4, RGBACode = "#0000ffff" },
                    new ColorMetadata { Id = 5, RGBACode = "#ffff00ff" },
                    new ColorMetadata { Id = 6, RGBACode = "#00ffffff" },
                    new ColorMetadata { Id = 7, RGBACode = "#ff00ffff" },
                    new ColorMetadata { Id = 8, RGBACode = "#fff000ff" },
                    new ColorMetadata { Id = 9, RGBACode = "#000fffff" }
                },

                Turmites = new TurmiteMetadata[]
                {
                    new TurmiteMetadata { X = 320, Y = 240, Direction = Turmite.TOP, StateId = 0, Name = "langton" }
                }

            };

            WorldMetadata.Save(Path.Combine(
                Resources.BasePath,
                "resources/worlds/default.xml"), world);
        }
    }
}
