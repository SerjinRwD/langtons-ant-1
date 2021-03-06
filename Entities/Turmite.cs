namespace langtons_ant_1.Entities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public enum TransitionTurn
    {
        Right,
        Left,
        NoTurn,
        UTurn
    }

    public class Transition
    {
        public int OnStateId { get; set; }
        public int OnColorId { get; set; }
        public TransitionTurn Turn { get; set; }
        public int NewColorId { get; set; }
        public int NewStateId { get; set; }
    }

    public class TransitionStateTable
    {
        public int Colors { get; set; } 
        public int States { get; set; }
        public Transition[] Transitions { get; set; }

        public static TransitionStateTable Load(string fileName)
        {
            var s = new XmlSerializer(typeof(TransitionStateTable));

            using(var f = File.OpenRead(fileName))
            {
                var o = s.Deserialize(f);

                return (TransitionStateTable)o;
            }
        }

        public static void Save(string fileName, TransitionStateTable table)
        {
            var s = new XmlSerializer(typeof(TransitionStateTable));

            using(var f = File.OpenWrite(fileName))
            {
                s.Serialize(f, table);
            }
        }
    }

    public struct TurmiteStateKey
    {
        public int StateId { get; set; }
        public int ColorId { get; set; }

        public void Set(int state, int color)
        {
            StateId = state;
            ColorId = color;
        }

        public TurmiteStateKey(int state, int color)
        {
            StateId = state;
            ColorId = color;
        }
    }

    public class Turmite
    {
        public const int RIGHT = 0;
        public const int TOP = 1;
        public const int LEFT = 2;
        public const int BOTTOM = 3;
        public int X { get; set; }
        public int Y { get; set; }
        public int StateId { get; set; }
        public int Direction { get; set; }
        public Dictionary<TurmiteStateKey, Transition> Table{ get; private set; }

        private TurmiteStateKey _occuringState = new TurmiteStateKey();

        public int UpdateState(int colorId)
        {
            _occuringState.Set(StateId, colorId);
            
            if(!Table.ContainsKey(_occuringState))
            {
                return 0;
            }

            var t = Table[_occuringState];

            // Новое направление
            switch(t.Turn)
            {
                case TransitionTurn.NoTurn:
                    break;

                case TransitionTurn.Left:
                    Direction = Direction >= BOTTOM ? RIGHT : Direction += 1;
                    break;

                case TransitionTurn.Right:
                    Direction = Direction <= RIGHT ? BOTTOM : Direction -= 1;
                    break;

                case TransitionTurn.UTurn:
                    Direction = (Direction + 2) % 4;
                    break;
            }

            // Перемещаемся
            switch(Direction)
            {
                case RIGHT:
                    X += 1;
                    break;
                case TOP:
                    Y -= 1;
                    break;
                case LEFT:
                    X -= 1;
                    break;
                case BOTTOM:
                    Y += 1;
                    break;
            }

            return t.NewColorId;
        }

        public Turmite(TransitionStateTable table)
        {
            Table = new Dictionary<TurmiteStateKey, Transition>();

            foreach(var t in table.Transitions)
            {
                Table.Add(new TurmiteStateKey(t.OnStateId, t.OnColorId), t);
            }
        }
    }
}