namespace langtons_ant_1.Entities
{
    public class TurmiteMetadata
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Direction { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
    }

    public class ColorMetadata
    {
        public int Id { get; set; }
        public string RGBCode { get; set; }
    }

    public class WorldMetadata
    {
        public int W { get; set; }
        public int H { get; set; }
        public TurmiteMetadata[] Turmites { get; set; }
        public ColorMetadata[] Colors { get; set; }
    }
}