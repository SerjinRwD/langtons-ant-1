namespace langtons_ant_1.Entities
{
    using System;

    public class Label
    {
        private int _x;
        private int _y;
        private string _text;

        public int X
        { 
            get => _x;
            set
            { 
                _x = value;
                LabelChanged?.Invoke(this, null);
            }
        }

        public int Y
        { 
            get => _y;
            set
            { 
                _y = value;
                LabelChanged?.Invoke(this, null);
            }
        }

        public string Text
        { 
            get => _text;
            set
            { 
                _text = value;
                LabelChanged?.Invoke(this, null);
            }
        }

        public event Action<object, EventArgs> LabelChanged;
    }
}