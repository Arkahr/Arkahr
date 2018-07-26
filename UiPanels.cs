using Turbo.Plugins.Default;
using System.Collections.Generic;
using SharpDX;

namespace Turbo.Plugins.Arkahr
{

    public class UiPanels : BasePlugin, IInGameTopPainter
    {               
        private List<Panel> Panels;

        public UiPanels()
        {
            Enabled = false;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            //test
            Panels.Add(new Panel(hud.Render.MinimapUiElement.Rectangle.Left, hud.Render.MinimapUiElement.Rectangle.Bottom, 48f, hud.Render.MinimapUiElement.Rectangle.Width));
        }

        public void PaintTopInGame(ClipState clipState)
        {            
            if (!Enabled) return;
            if (clipState != ClipState.BeforeClip) return;

            foreach (var panel in Panels)
            {                    
                Hud.Render.CreateBrush(255, 0, 0, 0, 0).DrawRectangle(new RectangleF(panel.X, panel.Y, panel.Width, panel.Height));     
            }
        }
    }
}