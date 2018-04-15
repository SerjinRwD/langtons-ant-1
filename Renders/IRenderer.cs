namespace langtons_ant_1.Renders
{
    using System;

    public interface IRenderer
    {
        void Render();
    }

    public interface IRenderer<T> : IRenderer
    { }
}