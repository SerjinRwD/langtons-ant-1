namespace langtons_ant_1.Entities
{
    using System.IO;    
    using System.Xml;
    using System.Xml.Serialization;

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
        public string RGBACode { get; set; }
    }

    public class WorldMetadata
    {
        public int W { get; set; }
        public int H { get; set; }
        public TurmiteMetadata[] Turmites { get; set; }
        public ColorMetadata[] Colors { get; set; }

        public static WorldMetadata Load(string fileName)
        {
            var s = new XmlSerializer(typeof(WorldMetadata));

            using(var f = File.OpenRead(fileName))
            {
                var o = s.Deserialize(f);

                return (WorldMetadata)o;
            }
        }

        public static void Save(string fileName, WorldMetadata table)
        {
            var s = new XmlSerializer(typeof(WorldMetadata));

            using(var f = File.OpenWrite(fileName))
            {
                s.Serialize(f, table);
            }
        }
    }
}