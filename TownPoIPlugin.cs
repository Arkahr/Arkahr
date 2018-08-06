using System.Linq;
using Turbo.Plugins.Default;
using System.Collections.Generic;
using System;

namespace Turbo.Plugins.Arkahr
{
   
    public class TownPoIPlugin : BasePlugin, IInGameWorldPainter
	{        
        public WorldDecoratorCollection UberMinimapDecorator;
        public WorldDecoratorCollection EnchantingCouldronDecorator;
        public WorldDecoratorCollection FollowerMinimapDecorator;
        public bool UberEnabled;
        public bool EnchantingCouldronEnabled; //enchanting npc
        public bool FollowersEnabled;

		public TownPoIPlugin()
		{
            Enabled = true;
            UberEnabled = true;
            EnchantingCouldronEnabled = true;
            FollowersEnabled = true;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);  
            
            UberMinimapDecorator = new WorldDecoratorCollection(
                new MapLabelDecorator(Hud)
                {                    
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 100, 100, true, false, 128, 0, 0, 0, false),
                    RadiusOffset = -20
                });      

            EnchantingCouldronDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {                    
                    BorderBrush = Hud.Render.CreateBrush(255, 223, 73, 223, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 8f, 255, 223, 73, 223, true, false, 128, 0, 0, 0, true),
                }); 

            FollowerMinimapDecorator = new WorldDecoratorCollection(
                new MapLabelDecorator(Hud)
                {                    
                    LabelFont = Hud.Render.CreateFont("tahoma", 5f, 255, 87, 253, 251, true, false, 128, 0, 0, 0, false),
                });                  
        }
		
		public void PaintWorld(WorldLayer layer)
		{
            if (!Hud.Game.IsInTown) return;

            // Uber Bosses Map text toggle on/off 
            if (UberEnabled) {
                var Uber = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.SnoActor.Sno == 258064);
                foreach (var uber in Uber)
                {
                    UberMinimapDecorator.Paint(layer, uber, uber.FloorCoordinate, "UberBosses");
                }
            }            

           // Enchanting Couldron toggle on/off 
            if (EnchantingCouldronEnabled) {               
                var EnchantingCouldron = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && x.NormalizedXyDistanceToMe >15 && x.SnoActor.Sno == 212511).ToList();
                
                if (EnchantingCouldron.Any()) EnchantingCouldronDecorator.Paint(layer, EnchantingCouldron.First(), EnchantingCouldron.First().FloorCoordinate, "Enchant");
            }           
            
            // Followers Enabled toggle on/off
            if (FollowersEnabled) {
                var followers = Hud.Game.Actors.Where(x => x.DisplayOnOverlay && (x.SnoActor.Sno == 4062 || x.SnoActor.Sno == 4538 ||x.SnoActor.Sno == 4644));
                if (followers.Any()) {   
                    var folower = followers.Last();                
                    FollowerMinimapDecorator.Paint(layer, folower, folower.FloorCoordinate, "Followers");                    
                }  
            }
             
		}      
    }
}