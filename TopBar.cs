using System.Linq;
using Turbo.Plugins.Default;
using SharpDX;

namespace Turbo.Plugins.Arkahr
{
    public class TopBar : BasePlugin, IInGameTopPainter
	{        
        private Panel topBarPanel;
        public HorizontalTopLabelList LabelList { get; private set; }


		public TopBar()
		{
            Enabled = false;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);                
        }
		
        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            if ((Hud.Game.MapMode == MapMode.WaypointMap) || (Hud.Game.MapMode == MapMode.ActMap)) return;

            LabelList.Paint();
        }
    }
}