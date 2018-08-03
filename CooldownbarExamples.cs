using System;
using System.Linq;
using System.Collections.Generic;
using Turbo.Plugins.Default;
using SharpDX;

namespace Turbo.Plugins.Arkahr
{
    public class CooldownbarExamples : BasePlugin, IInGameTopPainter
    {
        private IBrush barBrush;
        private IWatch timer;
        private List<IBrush> brushes;
        private BrushFromHex BrushesFromHex;
        
        public CooldownbarExamples()
        {
            Enabled = false;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            
            barBrush = hud.Render.CreateBrush(255,255,24,0,0);
            timer = hud.Time.CreateAndStartWatch();
            brushes = new List<IBrush>();
            BrushesFromHex = new BrushFromHex(hud);
            brushes.Add(BrushesFromHex.CreateSolidBrush(255,"ffaaff"));
            brushes.Add(BrushesFromHex.CreateSolidBrush(255,"fabafc"));
            brushes.Add(BrushesFromHex.CreateSolidBrush(255,"aa22ce"));
            brushes.Add(BrushesFromHex.CreateSolidBrush(255,"24ade2"));
            brushes.Add(BrushesFromHex.CreateSolidBrush(255,"42f3ae"));
            brushes.Add(BrushesFromHex.CreateSolidBrush(255,"003ad2"));
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.AfterClip) return;
            
            //var uiMinimap = Hud.Render.MinimapUiElement.Rectangle;
            // var x = uiMinimap.Left;
            // var y = uiMinimap.Bottom + 100f;
            // var w = uiMinimap.Width;
            // var h = 30f;

            var x = 500f;
            var y = 500f;
            var w = 300f;
            var h = 30f;

            // TODO works ! change to function

            foreach (var brush in brushes)         
            {
                if (timer.IsRunning && !timer.TimerTest(3000)) {
                    paintBar(new RectangleF(x,y,w,h), 3000 - timer.ElapsedMilliseconds, 3000, brush);
                }
                else {
                    timer.Reset();
                    timer.Start();
                }
                y += h+5f;
            }

            
        }



        public void paintBar(RectangleF rect, double timeleft, double duration, IBrush brush)
        {
            if (timeleft == 0) return;
            var width = rect.Width * (float)(timeleft/duration);
            brush.DrawRectangle(rect.X,rect.Y,width,rect.Height);
        }
        
    }

    public class CooldownBarTimeTest 
    {
        public IWatch timer;
        public IBrush BarBrush;

        public CooldownBarTimeTest(IController hud)
        {
            var r = new Random(hud.Time.Now.Millisecond);
            //co tu ?
        }
    }

}