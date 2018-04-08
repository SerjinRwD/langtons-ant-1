namespace langtons_ant_1.Entities
{
    using Entities;
    using SDL2;
    using System;
    using System.Collections.Generic;

    public class World
    {
        public int W { get; private set; }
        public int H { get; private set; }
        public Dictionary<string, TransitionStateTable> TablesCache { get; private set; }
        public SDL.SDL_Color[] Colors { get; private set; }
        public List<Turmite> Turmites { get; private set; }

        public World(WorldMetadata world)
        {

        }

        public void Update()
        {
            
        }
    }
}