using Turbo.Plugins.Default;
using SharpDX;

namespace Turbo.Plugins.Arkahr
{

    public class Panel
    {                       
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Panel(float x, float y, float w, float h)
        {            
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

    }
}