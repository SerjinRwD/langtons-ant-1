namespace langtons_ant_1.Entities
{
    using Entities;
    using langtons_ant_1.Helpers;
    using SDL2;
    using System;
    using System.Collections.Generic;

    public class WorldCellChangedEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int NewColorId { get; set; }

        public WorldCellChangedEventArgs(int currentX, int currentY, int currentColorId)
        {
            X = currentX;
            Y = currentY;
            NewColorId = currentColorId;
        }
    }

    public delegate void WorldCellChangedEvent(object sender, WorldCellChangedEventArgs args);

    public class World
    {
        public int W { get; private set; }
        public int H { get; private set; }
        public Dictionary<string, TransitionStateTable> TablesCache { get; private set; }
        public SDL.SDL_Color[] Colors { get; private set; }
        public List<Turmite> Turmites { get; private set; }
        public int[,] Cells { get; private set; }
        public event WorldCellChangedEvent WorldCellChanged;

        public World(WorldMetadata world)
        {
            W = world.W;
            H = world.H;

            Cells = new int[W, H];

            for(var x = 0; x < W; x++)
            {
                for(var y = 0; y < H; y++)
                {
                    Cells[x, y] = 0;
                }
            }

            Colors = new SDL.SDL_Color[world.Colors.Length];

            foreach(var cm in world.Colors)
            {
                Colors[cm.Id] = Resources.ParseColorCode(cm.RGBACode);
            }

            Turmites = new List<Turmite>();

            foreach(var tm in world.Turmites)
            {
                var tst = TransitionStateTable.Load(Resources.GetFilePath($@"turmites/{tm.Name}.xml"));
                var t = new Turmite(tst);

                t.Direction = tm.Direction;
                t.StateId = tm.StateId;
                t.X = tm.X;
                t.Y = tm.Y;

                Turmites.Add(t);
            }
        }

        public void Update()
        {
            if(Turmites == null || Turmites.Count == 0)
            {
                return;
            }

            foreach(var t in Turmites)
            {
                var currentX = t.X;
                var currentY = t.Y;

                Cells[currentX, currentY] = t.UpdateState(Cells[currentX, currentY]);

                WorldCellChanged?.Invoke(
                    this, new WorldCellChangedEventArgs(currentX, currentY, Cells[currentX, currentY]));

                if(t.X < 0)
                {
                    t.X = W - 1;
                }
                else if(t.X > W - 1)
                {
                    t.X = 0;
                }

                if(t.Y < 0)
                {
                    t.Y = H - 1;
                }
                else if(t.Y > H - 1)
                {
                    t.Y = 0;
                }
            }
        }
    }
}