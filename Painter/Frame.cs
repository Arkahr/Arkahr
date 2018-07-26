using Turbo.Plugins.Default;
using SharpDX;
using System.Collections.Generic;

namespace Turbo.Plugins.Arkahr {

        public class Frame {

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            private int barHeight;
            public int BarSpacing;
            public IFont TextFont;
            public IBrush BarBrush;
            public List<Bar> Bars;

            public Frame() {
                Bars = new List<Bar>();                
                BarSpacing = 1;
            }

            public void Paint() {
                var y = Y;
                foreach (var bar in Bars)
                {
                    bar.Paint(new RectangleF(X, y, Width, barHeight), BarBrush, TextFont);
                    y += BarSpacing;
                }
            }
       }
}