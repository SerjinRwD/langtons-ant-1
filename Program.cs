namespace langtons_ant_1
{
    using langtons_ant_1.Entities;
    using langtons_ant_1.Handlers;
    using langtons_ant_1.Helpers;
    using langtons_ant_1.Renders;

    using SDL2;
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        private static IntPtr _renderer;
        private static IntPtr _window;
        private static string _worldPath = Resources.GetFilePath("worlds/default.xml");
        private static World _world;
        private static WorldRenderer _worldRenderer;
        private static string _fontPath = Resources.GetFilePath("gfx/DejaVuSansMono-Oblique.ttf");
        private static IntPtr _font;
        private static KeyHandler KeydownHandler;
        private static bool _quit;
        private static int _steps;
        private static int _speed;
        private static int _delay;

        static void Main(string[] args)
        {
            try
            {
                Init();

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

            if(SDL_ttf.TTF_Init() != 0)
            {
                throw new SdlException(nameof(SDL_ttf.TTF_Init));
            }

            _window = SDL.SDL_CreateWindow("Langton`s Ant", 0, 0, 640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if(_window == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL.SDL_CreateWindow));
            }

            _renderer = SDL.SDL_CreateRenderer(
                _window, -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if(_renderer == IntPtr.Zero)
            {
                throw new SdlException(nameof(SDL.SDL_CreateRenderer));
            }

            _font = SDL_ttf.TTF_OpenFont(_fontPath, 14);

            KeydownHandler = new KeyHandler();
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_ESCAPE, () => _quit = true);
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_F5, LoadWorld);
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_F2, () => _speed -= 10);
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_F3, () => _speed += 10);
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_F4, () => _speed = 0);
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_z, () => GetWorldRenderer().AddZoom(-10));
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_x, () => GetWorldRenderer().AddZoom(10));
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_c, () => GetWorldRenderer().ResetZoom());
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_RIGHT, () => GetWorldRenderer().MoveZoom(10, 0));
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_UP, () => GetWorldRenderer().MoveZoom(0, -10));
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_LEFT, () => GetWorldRenderer().MoveZoom(-10, 0));
            KeydownHandler.Add(SDL.SDL_Keycode.SDLK_DOWN, () => GetWorldRenderer().MoveZoom(0, 10));
        }

        private static WorldRenderer GetWorldRenderer()
        {
            return _worldRenderer;
        }

        private static void LoadWorld()
        {
            _quit = false;
            _steps = 0;
            _speed = 1;

            _world = new World(WorldMetadata.Load(_worldPath));
            _worldRenderer = new WorldRenderer(_renderer, _world);
        }

        private static void Run()
        {
            LoadWorld();

            var localRenders = new List<IRenderer>();

            var label = new Label();
            label.Text = "Шаг: ";
            label.X = 16;
            label.Y = 16;

            var label2 = new Label();
            label2.Text = "0";
            label2.X = 64;
            label2.Y = 16;

            localRenders.AddRange(new IRenderer[]
            {
                new LabelRenderer(_renderer, _font, Resources.ParseColorCode("#ffff00ff"), label),
                new LabelRenderer(_renderer, _font, Resources.ParseColorCode("#ffff00ff"), label2)
            });

            var e = new SDL.SDL_Event();

            while(!_quit)
            {
                // event handling
                while(SDL.SDL_PollEvent(out e) > 0)
                {
                    if(e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        _quit = true;
                    }

                    if(e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        KeydownHandler.Handle(e.key.keysym.sym);
                    }
                }

                label2.Text = _steps.ToString();

                if(_speed == 0)
                {
                    _world.Update();
                    _steps++;
                }
                else if(_speed > 0)
                {
                    for(var i = 0; i < _speed; i++)
                    {
                        _world.Update();
                    }
                    _steps += _speed;
                }
                else
                {
                    if(_delay <= 0)
                    {
                        _delay = -_speed;
                        _world.Update();
                        _steps++;
                    }
                    else
                    {
                        _delay--;
                    }
                }

                // rendering
                SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
                SDL.SDL_RenderClear(_renderer);

                _worldRenderer.Render();
                localRenders.ForEach(r => r.Render());

                SDL.SDL_RenderPresent(_renderer);
            }
        }

        private static void Quit()
        {
            if(!(_font == IntPtr.Zero))
            {
                SDL_ttf.TTF_CloseFont(_font);
            }

            if(!(_renderer == IntPtr.Zero))
            {
                SDL.SDL_DestroyRenderer(_renderer);
            }

            if(!(_window == IntPtr.Zero))
            {
                SDL.SDL_DestroyWindow(_window);
            }

            SDL_ttf.TTF_Quit();
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
