using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Arkahr
{
    public class TownPoI : BasePlugin, IInGameWorldPainter
	{        
        public WorldDecoratorCollection KadalaDecorator { get; set; }
        public WorldDecoratorCollection MyriamDecorator { get; set; }
        public bool matsEnabled { get; set; }
        public bool KadalaEnabled { get; set; }
        public bool MyriamEnabled { get; set; } //enchanting npc

        
		public TownPoI()
		{
            Enabled = true;
            KadalaEnabled = true;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);   
            
            KadalaDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(200, 255, 100, 100, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 8f, 255, 255, 100, 100, true, false, 128, 0, 0, 0, true),
                }
                );      

            MyriamDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(200, 255, 216, 0, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 8f, 255, 255, 216, 0, true, false, 128, 0, 0, 0, true),
                }
                );
                
        }
		
		public void PaintWorld(WorldLayer layer)
		{
            // Kadala Label toggle on/off 
            if (KadalaEnabled) {
                var Kadala = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.NameEnglish == "Kadala");
                foreach (var actor in Kadala)
                {
                    KadalaDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized + " " + Hud.Game.Me.Materials.BloodShard);
                }
            }
            

           // Myriam Label toggle on/off
           if (MyriamEnabled) {
                var Myriam = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.NameEnglish == "Enchant");
                foreach (var actor in Myriam)
                {
                    MyriamDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);
                }
            }

            //Minimap GR keys counter
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



            /* var MaterialCountFont = Hud.Render.CreateFont("tahoma", 6, 255, 190, 190, 190, false, false, false);
            var layout = MaterialCountFont.GetTextLayout(ValueToString(count, ValueFormat.NormalNumberNoDecimal));
            MaterialCountFont.DrawText(layout, x,y); */

            var GRFont = Hud.Render.CreateFont("tahoma", 6, 255, 61, 29, 91, true, false, false);
            var GRlayout = GRFont.GetTextLayout(ValueToString(count, ValueFormat.NormalNumberNoDecimal));
            GRFont.DrawText(GRlayout, x,y);

            }
          
		}
    }
}