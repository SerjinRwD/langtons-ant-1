using System;
using SDL2;

namespace langtons_ant_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var tst = new Classes.TransitionStateTable
            {
                Colors = 2,
                States = 1,
                Transitions = new Classes.Transition[]
                {
                    new Classes.Transition
                    {
                        OnStateId = 0,
                        OnColorId = 0,
                        Turn = Classes.TransitionTurn.Left,
                        NewColorId = 1,
                        NewStateId = 0,
                    },

                    new Classes.Transition
                    {
                        OnStateId = 0,
                        OnColorId = 1,
                        Turn = Classes.TransitionTurn.Right,
                        NewColorId = 0,
                        NewStateId = 0,
                    }
                }
            };

            var path = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location),
                "out.xml");

            var s = new System.Xml.Serialization.XmlSerializer(typeof(Classes.TransitionStateTable));

            s.Serialize(System.IO.File.OpenWrite(path), tst);
        }
    }
}
