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
        
        public void GRKeys() 
        {

            //GR keys counter -> move to TopBar plugin
            if (Hud.Game.IsInTown) {
                var uiRect = Hud.Render.InGameBottomHudUiElement.Rectangle;
                var snoItem = Hud.Inventory.GetSnoItem(2835237830);

                var x = uiRect.Right - 305f;
                var y = uiRect.Bottom -13f;

                var count = 0;
                foreach (var item in Hud.Inventory.ItemsInStash)
                {
                    if (item.SnoItem == snoItem) count += (int)item.Quantity;
                }

                foreach (var item in Hud.Inventory.ItemsInInventory)
                {
                    if (item.SnoItem == snoItem) count += (int)item.Quantity;
                }

                var GRFont = Hud.Render.CreateFont("tahoma", 6, 255, 61, 29, 91, true, false, false);
                var GRlayout = GRFont.GetTextLayout(ValueToString(count, ValueFormat.NormalNumberNoDecimal));
                GRFont.DrawText(GRlayout, x,y);
            }

        }
    }
}